#ifndef CONTROLE_BIOMETRICO_H
#define CONTROLE_BIOMETRICO_H

// Inclui bibliotecas necessárias para o funcionamento dos componentes
#include <Adafruit_Fingerprint.h> // Biblioteca para leitura de impressões digitais
#include <LiquidCrystal_I2C.h>   // Biblioteca para controle de display LCD via I2C
#include <Ticker.h>              // Biblioteca para controle de temporizadores
#include <MQTTManager.h>         // Gerenciamento de comunicação MQTT (publicar/assinar tópicos)

class ControleBiometrico {
private:
    // Configuração da comunicação serial com o sensor de impressão digital
    HardwareSerial fingerSerial;
    Adafruit_Fingerprint finger; // Instância para interagir com o sensor de impressão digital
    LiquidCrystal_I2C lcd;       // Instância para controle do display LCD
    MQTTManager* mqttManager;    // Gerenciador de comunicação MQTT

    // Pinos do hardware
    const int LED_ENTRAR; // LED verde, indica acesso permitido
    const int LED_RECUSAR; // LED vermelho, indica acesso negado
    const int CATRACA;     // Controle da catraca ou trava elétrica

    // Estado dos LEDs
    bool estadoLedVerde;     // Estado do LED verde
    bool estadoLedVermelho;  // Estado do LED vermelho
    bool piscandoLeds;       // Indica se os LEDs estão piscando

    // Modo de operação do sistema (pode ser usado para alternar entre modos, como cadastro ou verificação)
    char modo;

    // Instâncias de Ticker para controlar eventos baseados em tempo
    Ticker lcdTicker;        // Atualização do LCD
    Ticker ledEntrarTicker;  // Controle do tempo do LED verde
    Ticker ledRecusarTicker; // Controle do tempo do LED vermelho
    Ticker ledPiscarTicker;  // Controle do piscar dos LEDs

    // Métodos privados, usados internamente pela classe
    void processarComando(char comando); // Processa comandos recebidos
    void atualizarLCD();                 // Atualiza mensagens no display LCD
    void desligarLedEntrar();            // Desliga o LED verde
    void desligarLedRecusar();           // Desliga o LED vermelho
    void piscarLeds();                   // Controla o piscar dos LEDs
    void pararPiscarLeds();              // Para o piscar dos LEDs
    uint8_t getFingerprintEnroll(uint8_t id); // Registra uma nova impressão digital
    uint8_t getFingerprintID();               // Obtém o ID de uma impressão digital lida
    uint8_t apagarDigitalPorID(uint8_t id);   // Apaga uma impressão digital por ID
    uint8_t apagarTodasDigitais();            // Apaga todas as impressões digitais registradas
    uint8_t readnumber();                     // Lê números da entrada serial

    // Métodos estáticos para callbacks de Ticker
    static void atualizarLCDStatic(ControleBiometrico* instance);   // Callback para atualizar o LCD
    static void desligarLedEntrarStatic(ControleBiometrico* instance); // Callback para desligar o LED verde
    static void desligarLedRecusarStatic(ControleBiometrico* instance); // Callback para desligar o LED vermelho
    static void piscarLedsStatic(ControleBiometrico* instance);     // Callback para controlar o piscar dos LEDs

public:
    // Construtor da classe
    ControleBiometrico(int rx_pin, int tx_pin); // Inicializa o sensor de impressão digital nos pinos RX e TX

    // Métodos públicos, acessíveis fora da classe
    void setMQTTManager(MQTTManager* manager) { mqttManager = manager; } // Configura o gerenciador MQTT
    void inicializar();      // Inicializa os componentes do sistema
    void loop();             // Método principal para execução contínua
    void mostrarMenu();      // Exibe o menu no display LCD
    void exibirMensagemNoLCD(const String& linha1, const String& linha2); // Exibe mensagens no LCD
    void controlarLEDs(bool verde, bool vermelho, bool rele, int duracao = 2000); // Controla os LEDs e a catraca
    void iniciarPiscarLeds(); // Inicia o piscar dos LEDs
};

#endif
