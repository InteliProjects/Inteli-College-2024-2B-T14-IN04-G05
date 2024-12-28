// Diretiva de guarda para evitar inclusões múltiplas
#ifndef MQTT_MANAGER_H
#define MQTT_MANAGER_H

// Inclusão das bibliotecas necessárias
#include <WiFi.h>           // Biblioteca para conexão WiFi
#include <WiFiClientSecure.h> // Cliente WiFi seguro
#include <PubSubClient.h>   // Cliente MQTT
#include <ArduinoJson.h>    // Manipulação de JSON

// Definição da classe MQTTManager para gerenciamento de conexões
class MQTTManager {
private:
    // Credenciais WiFi
    const char* ssid;           // Nome da rede WiFi
    const char* password;       // Senha da rede WiFi
    
    // Configurações do servidor MQTT
    const char* mqtt_server;    // Endereço do broker MQTT
    const int mqtt_port;        // Porta do broker MQTT
    const char* mqtt_user;      // Usuário MQTT (opcional)
    const char* mqtt_password;  // Senha MQTT (opcional)
    
    // Clientes para conexão
    WiFiClientSecure espClient; // Cliente WiFi seguro
    PubSubClient client;        // Cliente MQTT
    
    // Métodos privados de gerenciamento interno
    void setupWiFi();           // Configura conexão WiFi
    bool reconnect();           // Reconecta ao broker MQTT
    static void callback(char* topic, byte* payload, unsigned int length); // Callback para mensagens MQTT
    bool checkWiFiConnection(); // Verifica estado da conexão WiFi

public:
    // Construtor com parâmetros padrão
    MQTTManager(
        const char* wifi_ssid,           // SSID da rede
        const char* wifi_password,       // Senha da rede
        const char* mqtt_server,         // Servidor MQTT
        int mqtt_port = 1883,            // Porta MQTT padrão
        const char* mqtt_user = nullptr, // Usuário MQTT opcional
        const char* mqtt_password = nullptr // Senha MQTT opcional
    );
    
    // Métodos públicos para publicação de eventos
    void publicarCadastro(const char* tipo, int id, const char* status);  // Publica evento de cadastro
    void publicarRemocao(const char* tipo, int id);                       // Publica evento de remoção
    
    // Métodos de gerenciamento do MQTT
    void inicializar();     // Inicializa conexões
    void loop();            // Mantém conexões ativas
    void publicarAcesso(const char* tipo, int id, const char* status); // Publica evento de acesso
};
#endif // Fim da diretiva de guarda