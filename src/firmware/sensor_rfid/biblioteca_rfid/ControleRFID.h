#ifndef ControleRFID_h
#define ControleRFID_h

// Inclusão das bibliotecas necessárias
#include <Arduino.h>
#include <SPI.h>
#include <MFRC522.h>
#include <EEPROM.h>
#include <Ticker.h>
#include <MQTTManager.h>
#include <LiquidCrystal_I2C.h>  

class ControleRFID {
  private:
    // Componentes e objetos do sistema
    MFRC522 rfid;  // Módulo RFID para leitura de cartões
    
    // Timers para diferentes funções (porta, LEDs, buzzer)
    Ticker portaTimer;
    Ticker ledEntrarTicker;
    Ticker ledRecusarTicker;
    Ticker ledPiscarTicker;
    Ticker buzzerTicker;  // Timer específico para o buzzer
    LiquidCrystal_I2C lcd;       // Instância para controle do display LCD
    
    MQTTManager* mqttManager;  // Ponteiro para gerenciamento MQTT
   
    // Constantes e configurações do sistema
    const int MAX_CARTOES = 10;  // Número máximo de cartões permitidos
    int numCartoesRegistrados = 0;  // Contador de cartões cadastrados
    
    // Flags de controle de estado
    bool modoCadastro = false;  // Indica se está em modo de cadastro
    bool portaAberta = false;   // Indica se a porta está aberta
    bool estadoLedVerde = false;  // Estado do LED verde
    bool estadoLedVermelho = false;  // Estado do LED vermelho
    bool piscandoLeds = false;  // Indica se os LEDs estão piscando
   
    // Pinos de configuração
    uint8_t pinSS;     // Pino de seleção de escravo SPI
    uint8_t pinRST;    // Pino de reset
    uint8_t pinRELE;   // Pino do relé
    uint8_t LED_ENTRAR;   // Pino do LED verde (acesso permitido)
    uint8_t LED_RECUSAR;  // Pino do LED vermelho (acesso negado)
    uint8_t BUZZER_PIN;   // Pino do buzzer
   
    // Métodos privados para operações internas
    void mostrarMenu();  // Exibe opções de menu
    void cadastrarNovoCartao();  // Cadastra um novo cartão
    void removerCartao();  // Remove um cartão existente
    void listarCartoes();  // Lista cartões cadastrados
    void apagarTodosCartoes();  // Apaga todos os cartões
    void mostrarUID(byte* uid);  // Mostra UID do cartão
    bool comparaUID(byte* uid1, byte* uid2);  // Compara UIDs
    bool verificaAcesso(byte* uid);  // Verifica se cartão tem acesso
    void executarComando(char comando);  // Processa comandos
    
    // Métodos de controle de LEDs e buzzer
    void piscarLeds();  // Faz LEDs piscarem
    void desligarLedEntrar();  // Desliga LED verde
    void desligarLedRecusar();  // Desliga LED vermelho
    void tocarBuzzerAcesso();  // Toca buzzer para acesso
    void tocarBuzzerNegado();  // Toca buzzer para acesso negado
    void desligarBuzzer();  // Desliga buzzer
   
  public:
    // Construtor com configuração de pinos
    ControleRFID(uint8_t ss_pin, uint8_t rst_pin, uint8_t rele_pin, 
                 uint8_t led_verde_pin, uint8_t led_vermelho_pin, 
                 uint8_t buzzer_pin);
    
    bool inicializar();  // Inicializa o sistema
    void loop();  // Laço principal de execução
    
    // Configura gerenciador MQTT
    void setMQTTManager(MQTTManager* manager) { mqttManager = manager; }
   
    // Métodos públicos de controle
    void controlarLEDs(bool verde, bool vermelho, bool rele, int duracao = 3000);
    void iniciarPiscarLeds();  // Inicia piscada de LEDs
    void exibirMensagemNoLCD(const String& linha1, const String& linha2); // Exibe mensagens no LCD
    void pararPiscarLeds();    // Para piscada de LEDs
    void fecharPorta();  // Fecha a porta
    void abrirPorta();   // Abre a porta
};
#endif