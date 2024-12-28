#include "ControleRFID.h"

// Pinos para o RFID
#define SS_PIN 5
#define RST_PIN 4
#define RELE_PIN 12

ControleRFID controleRFID(SS_PIN, RST_PIN, RELE_PIN);

void setup() {
    
    // Inicializa o controle RFID
    if (!controleRFID.inicializar()) {
        Serial.println("Erro ao inicializar RFID!");
        while(1); // Trava o programa se houver erro
    }
}

void loop() {

    // Executa as rotinas do RFID
    controleRFID.loop();
}