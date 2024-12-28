#include "ControleBiometrico.h"

// Define os pinos de comunicação com o sensor
#define RX_PIN 16
#define TX_PIN 17

// Cria uma instância do controle biométrico
ControleBiometrico controleBiometrico(RX_PIN, TX_PIN);

void setup() {
    // Código pra inicializar o biométrico
    controleBiometrico.inicializar();
}

void loop() {
    // Código para fazer em loop o biométrico
    controleBiometrico.loop();
}