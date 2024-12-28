#include "MQTTManager.h"

// Construtor da classe MQTTManager
MQTTManager::MQTTManager(
    const char* wifi_ssid,
    const char* wifi_password,
    const char* mqtt_server,
    int mqtt_port,
    const char* mqtt_user,
    const char* mqtt_password
) : ssid(wifi_ssid),
    password(wifi_password),
    mqtt_server(mqtt_server),
    mqtt_port(mqtt_port),
    mqtt_user(mqtt_user),
    mqtt_password(mqtt_password),
    client(espClient)
{
    // Configuração de conexão insegura para cliente WiFi
    espClient.setInsecure();
    Serial.println("[INIT] Inicializando MQTTManager");
}

// Configuração de conexão WiFi
void MQTTManager::setupWiFi() {
    // Verifica se já está conectado
    if (WiFi.status() == WL_CONNECTED) {
        Serial.println("[WIFI] Já conectado");
        return;
    }
    
    Serial.print("[WIFI] Conectando a ");
    Serial.println(ssid);
    
    // Configura modo estação
    WiFi.mode(WIFI_STA);
    WiFi.begin(ssid, password);
    
    
}

// Method to calculate WiFi signal strength in dBm
int getWiFiSignalStrengthDBm() {
    // Check if WiFi is connected
    if (WiFi.status() != WL_CONNECTED) {
        return 0; // Or a specific error value
    }
    
    // Get RSSI (Received Signal Strength Indicator)
    int32_t rssi = WiFi.RSSI();
    
    // Optional: Add some logging
    Serial.print("[WIFI] Signal Strength: ");
    Serial.print(rssi);
    Serial.println(" dBm");
    
    return rssi;
}

// Optional method to categorize signal strength
const char* getWiFiSignalQuality(int dbm) {
    if (dbm >= -50) return "Excellent";
    if (dbm >= -60) return "Good";
    if (dbm >= -70) return "Fair";
    if (dbm >= -80) return "Weak";
    return "Very Weak";
}

// Verifica conexão WiFi
bool MQTTManager::checkWiFiConnection() {
    if (WiFi.status() != WL_CONNECTED) {
        Serial.println("[WIFI] Conexão perdida. Tentando reconectar...");
        setupWiFi();
    }
    return WiFi.status() == WL_CONNECTED;
}

std::function<void(const char*)> customCallback;

void MQTTManager::setCustomCallback(std::function<void(const char*)> cb)
{
    customCallback = cb;
}

void MQTTManager::callback(char* topic, byte* payload, unsigned int length) {
    // Convert payload to string
    char message[length + 1];
    memcpy(message, payload, length);
    message[length] = '\0';

    Serial.print("[MQTT] Received on topic: ");
    Serial.println(topic);
    Serial.print("[MQTT] Message: ");
    Serial.println(message);

    // Check multiple topics
    if (strcmp(topic, "instituto/biometrico/comando") == 0 || 
        strcmp(topic, "instituto/rfid/comando") == 0)
    {
        // Call custom callback if defined
        if (customCallback)
        {
            customCallback(message);
        }
    }
}

// Inicialização do gerenciador MQTT
void MQTTManager::inicializar() {
    Serial.println("[MQTT] Inicializando conexão");
    
    // Conecta ao WiFi
    setupWiFi();
    
    // Configura servidor MQTT
    client.setServer(mqtt_server, mqtt_port);
    
    // Define callback para mensagens
    client.setCallback(callback);
    
    client.subscribe("instituto/biometrico/comando");
    client.subscribe("instituto/rfid/comando");
    
    Serial.println("[MQTT] Inicialização concluída");
}

// Reconexão ao broker MQTT
bool MQTTManager::reconnect() {
    // Gera ID de cliente único
    String clientId = "ESP32Client-" + String(random(0xffff), HEX);
    
    Serial.print("[MQTT] Tentando reconexão. Cliente ID: ");
    Serial.println(clientId);
    
    // Tenta conectar
    if (client.connect(clientId.c_str(), mqtt_user, mqtt_password)) {
        Serial.println("[MQTT] Reconectado com sucesso");
        return true;
    }
    
    Serial.println("[MQTT] Falha na reconexão");
    return false;
}

// Loop principal de gerenciamento
void MQTTManager::loop() {
    static unsigned long lastWiFiAttempt = 0;
    static unsigned long lastMQTTAttempt = 0;
    static unsigned long wifiStartTime = 0;
    static bool wifiConnecting = false;
    unsigned long currentTime = millis();

    // Gerenciamento da conexão WiFi
    if (WiFi.status() != WL_CONNECTED) {
        if (!wifiConnecting) {
            int signalStrength = getWiFiSignalStrengthDBm();
            const char* signalQuality = getWiFiSignalQuality(signalStrength);
            // Inicia nova tentativa de conexão
            if (currentTime - lastWiFiAttempt > 10000) { // 10 segundos entre tentativas
                Serial.println("[WIFI] Iniciando nova tentativa de conexão");
                setupWiFi();
                wifiConnecting = true;
                wifiStartTime = currentTime;
                lastWiFiAttempt = currentTime;
            }
        } else {
            // Verifica timeout da tentativa atual
            if (currentTime - wifiStartTime > 20000) { // 20 segundos de timeout
                Serial.println("[WIFI] Timeout na conexão");
                WiFi.disconnect();
                wifiConnecting = false;
            }
        }
        return; // Não tenta MQTT se não houver WiFi
    } else {
        // WiFi conectado com sucesso
        if (wifiConnecting) {
            Serial.println("[WIFI] Conectado com sucesso!");
            Serial.print("[WIFI] IP: ");
            Serial.println(WiFi.localIP());
            wifiConnecting = false;
        }
    }

    // Gerenciamento MQTT
     if (!client.connected()) {
        unsigned long currentTime = millis();
        if (currentTime - lastMQTTAttempt > 5000) { // Try every 5 seconds
            Serial.println("[MQTT] Attempting to reconnect");
            if (reconnect()) {
                // Resubscribe to topics
                client.subscribe("instituto/biometrico/comando");
                client.subscribe("instituto/rfid/comando");
            }
            lastMQTTAttempt = currentTime;
        }
    } else {
        client.loop(); // Process incoming messages
    }
}

// Métodos de publicação permanecem inalterados
void MQTTManager::publicarAcesso(const char* tipo, int id, const char* status) {
    // Tentativa de publicação, mas não bloqueia se falhar
    if (client.connected() && WiFi.isConnected()) {
        StaticJsonDocument<200> doc;
        doc["tipo"] = tipo;
        doc["id"] = id;
        doc["status"] = status;
        
        char jsonBuffer[200];
        serializeJson(doc, jsonBuffer);
        
        String topic = String("instituto/acesso/") + tipo + "/" + String(id);
        
        if (client.publish(topic.c_str(), jsonBuffer)) {
            Serial.print("[ACESSO] Publicado em: ");
            Serial.println(topic);
        } else {
            Serial.println("[ACESSO] Falha ao publicar");
        }
    }
}

// Métodos de publicarCadastro e publicarRemocao seguem o mesmo padrão
void MQTTManager::publicarCadastro(const char* tipo, int id, const char* status) {
    if (client.connected() && WiFi.isConnected()) {
        StaticJsonDocument<200> doc;
        doc["tipo"] = tipo;
        doc["id"] = id;
        doc["status"] = status;
        doc["evento"] = "cadastro";

        char jsonBuffer[200];
        serializeJson(doc, jsonBuffer);

        String topic = String("instituto/cadastro/") + tipo + "/" + String(id);
        
        if (client.publish(topic.c_str(), jsonBuffer)) {
            Serial.print("[CADASTRO] Publicado em: ");
            Serial.println(topic);
        } else {
            Serial.println("[CADASTRO] Falha ao publicar");
        }
    }
}

void MQTTManager::publicarRemocao(const char* tipo, int id) {
    if (client.connected() && WiFi.isConnected()) {
        StaticJsonDocument<200> doc;
        doc["tipo"] = tipo;
        doc["id"] = id;
        doc["evento"] = "remocao";

        char jsonBuffer[200];
        serializeJson(doc, jsonBuffer);

        String topic = String("instituto/remocao/") + tipo + "/" + String(id);
        
        if (client.publish(topic.c_str(), jsonBuffer)) {
            Serial.print("[REMOCAO] Publicado em: ");
            Serial.println(topic);
        } else {
            Serial.println("[REMOCAO] Falha ao publicar");
        }
    }
}