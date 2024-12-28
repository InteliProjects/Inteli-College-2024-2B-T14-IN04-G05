#include "ControleBiometrico.h"  // Biblioteca para controle do sensor biométrico 
#include "ControleRFID.h"        // Biblioteca para controle do módulo RFID
#include "MQTTManager.h"         // Biblioteca para gerenciar a conexão MQTT
#include "config_secrets.h"      // Arquivo de configuração com informações sensíveis (WiFi e MQTT)

// Definição dos pinos para o sensor biométrico
#define RX_PIN 16  // Pino RX conectado ao sensor biométrico
#define TX_PIN 17  // Pino TX conectado ao sensor biométrico

// Definição dos pinos para o módulo RFID e periféricos
#define SS_PIN 5             // Pino Slave Select (SS) para comunicação SPI com o módulo RFID
#define RST_PIN 4            // Pino de Reset do módulo RFID
#define RELE_PIN 26          // Pino conectado ao relé para controle de acesso
#define LED_VERDE_PIN 13     // Pino do LED verde (acesso permitido)
#define LED_VERMELHO_PIN 25  // Pino do LED vermelho (acesso negado)
#define BUZZER_PIN 14        // Pino do buzzer para sinal sonoro

#define RESET_BUTTON_PIN 34  // Pino do botão de reset (lado oposto da ESP)


// Instanciação do controle do sensor biométrico
ControleBiometrico controleBiometrico(RX_PIN, TX_PIN);

// Instanciação do controle do módulo RFID
ControleRFID controleRFID(SS_PIN, RST_PIN, RELE_PIN, LED_VERDE_PIN, LED_VERMELHO_PIN, BUZZER_PIN);

// Instanciação do gerenciador MQTT com as credenciais definidas no arquivo de configuração
MQTTManager mqttManager(WIFI_SSID, WIFI_PASSWORD, MQTT_SERVER, MQTT_PORT, MQTT_USER, MQTT_PASSWORD);

void setup() {
    // Inicializa a comunicação Serial
    Serial.begin(115200);

    // Configura o botão como entrada
    pinMode(RESET_BUTTON_PIN, INPUT);  // Pinos 34 e 35 não têm pull-up/pull-down interno

    // Inicializa a conexão MQTT (sem bloquear operações)
    mqttManager.inicializar();

    // Configura o gerenciador MQTT para o controle biométrico
    controleBiometrico.setMQTTManager(&mqttManager);

    // Inicializa o controle biométrico
    controleBiometrico.inicializar();

    // Inicializa o controle RFID e verifica se a inicialização foi bem-sucedida
    if (!controleRFID.inicializar()) {
        Serial.println("Erro ao inicializar RFID!");  // Exibe mensagem de erro no console
        // Não para a execução completamente
    }

    // Configura o gerenciador MQTT para o controle RFID
    controleRFID.setMQTTManager(&mqttManager);
}

void loop() {
    // Verifica o estado do botão de reset
    if (digitalRead(RESET_BUTTON_PIN) == LOW) {  // Botão pressionado
        Serial.println("Botão de reset pressionado. Reiniciando...");
        delay(100); // Pequeno delay para evitar múltiplas leituras falsas
        esp_restart(); // Reinicia a ESP32
    }

    // Executa as rotinas do controle biométrico (sempre)
    controleBiometrico.loop();

    // Executa as rotinas do gerenciador MQTT (tenta reconexão se necessário)
    mqttManager.loop();

    // Executa as rotinas do controle RFID (sempre)
    controleRFID.loop();

    // Pequeno delay para evitar sobrecarga do processador
    delay(1);
}
