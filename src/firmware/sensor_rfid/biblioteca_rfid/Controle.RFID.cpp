#include "ControleRFID.h"

// Construtor personalizado que inicializa os pinos do sistema RFID, incluindo o buzzer
ControleRFID::ControleRFID(uint8_t ss_pin, uint8_t rst_pin, uint8_t rele_pin, uint8_t led_verde_pin, uint8_t led_vermelho_pin, uint8_t buzzer_pin)
    : rfid(ss_pin, rst_pin),
      pinSS(ss_pin),
      pinRST(rst_pin),
      lcd(0x27, 16, 2),
      pinRELE(rele_pin),
      LED_ENTRAR(led_verde_pin),
      LED_RECUSAR(led_vermelho_pin),
      BUZZER_PIN(buzzer_pin),
      estadoLedVerde(false),
      estadoLedVermelho(false),
      piscandoLeds(false)
{
}

// Inicialização do sistema RFID
bool ControleRFID::inicializar()
{
  Serial.begin(115200);

  // Inicializa a EEPROM para armazenar cartões autorizados
  if (!EEPROM.begin(512))
  {
    Serial.println("Falha ao inicializar EEPROM");
    return false;
  }

  // Inicializa o barramento SPI e o módulo RFID
  SPI.begin();
  rfid.PCD_Init();

  // Inicializa o display LCD e ativa a luz de fundo
  lcd.init();
  lcd.backlight();

  // Configura os pinos de saída para relé, LEDs e buzzer
  pinMode(pinRELE, OUTPUT);
  pinMode(LED_ENTRAR, OUTPUT);
  pinMode(LED_RECUSAR, OUTPUT);
  pinMode(BUZZER_PIN, OUTPUT);

  // Define estado inicial dos pinos
  digitalWrite(pinRELE, LOW);
  digitalWrite(LED_ENTRAR, LOW);
  digitalWrite(LED_RECUSAR, LOW);
  digitalWrite(BUZZER_PIN, LOW);

  // Carrega o número de cartões registrados da EEPROM
  numCartoesRegistrados = EEPROM.read(0);
  if (numCartoesRegistrados > MAX_CARTOES)
  {
    // Reseta o contador se exceder o máximo
    numCartoesRegistrados = 0;
    EEPROM.write(0, 0);
    EEPROM.commit();
  }

  Serial.println("\nSistema RFID inicializado!");
  mostrarMenu();
  return true;
}

// Laço principal de controle do sistema
void ControleRFID::loop()
{
  // Verifica comandos seriais
  if (Serial.available() > 0)
  {
    char comando = Serial.read();
    if (comando != '\n' && comando != '\r')
    {
      executarComando(comando);
    }
  }

  // Modo de cadastro de novos cartões
  if (modoCadastro)
  {
    if (!piscandoLeds)
    {
      iniciarPiscarLeds();
    }
    if (rfid.PICC_IsNewCardPresent())
    {
      if (rfid.PICC_ReadCardSerial())
      {
        cadastrarNovoCartao();
        tocarBuzzerAcesso(); // Toca buzzer ao cadastrar cartão
        rfid.PICC_HaltA();
        rfid.PCD_StopCrypto1();
        modoCadastro = false;
        pararPiscarLeds();
        mostrarMenu();
      }
    }
  }
  // Modo de verificação de acesso
  else if (rfid.PICC_IsNewCardPresent())
  {
    if (rfid.PICC_ReadCardSerial())
    {
      // Verifica se o cartão tem acesso permitido
      if (verificaAcesso(rfid.uid.uidByte))
      {
        Serial.println("Acesso Permitido!");
        controlarLEDs(true, false, true);
        tocarBuzzerAcesso();
        // Publica evento de acesso via MQTT, se configurado
        if (mqttManager)
        {
          int uid_int = (rfid.uid.uidByte[0] << 24) |
                        (rfid.uid.uidByte[1] << 16) |
                        (rfid.uid.uidByte[2] << 8) |
                        rfid.uid.uidByte[3];
                        mqttManager->publicarAcesso("rfid", uid_int, "acesso_permitido");
        }
      }
      else
      {
        Serial.println("Acesso Negado!");
        controlarLEDs(false, true, false);
        tocarBuzzerNegado();
      }
      rfid.PICC_HaltA();
      rfid.PCD_StopCrypto1();
    }
  }
}

// Exibe uma mensagem no LCD
void ControleRFID::exibirMensagemNoLCD(const String &linha1, const String &linha2)
{
    lcd.clear();
    lcd.setCursor(0, 0);
    lcd.print(linha1);
    lcd.setCursor(0, 1);
    lcd.print(linha2);
}

// Funções para tocar buzzer em diferentes situações
void ControleRFID::tocarBuzzerAcesso()
{
  digitalWrite(BUZZER_PIN, HIGH);
  buzzerTicker.once_ms(200, std::bind(&ControleRFID::desligarBuzzer, this)); // Beep curto para acesso permitido
}

void ControleRFID::tocarBuzzerNegado()
{
  // Três beeps curtos para acesso negado
  digitalWrite(BUZZER_PIN, HIGH);
  buzzerTicker.once_ms(100, [this]()
                       {
        digitalWrite(BUZZER_PIN, LOW);
        buzzerTicker.once_ms(100, [this]() {
            digitalWrite(BUZZER_PIN, HIGH);
            buzzerTicker.once_ms(100, [this]() {
                digitalWrite(BUZZER_PIN, LOW);
                buzzerTicker.once_ms(100, [this]() {
                    digitalWrite(BUZZER_PIN, HIGH);
                    buzzerTicker.once_ms(100, std::bind(&ControleRFID::desligarBuzzer, this));
                });
            });
        }); });
}

// Desliga o buzzer
void ControleRFID::desligarBuzzer()
{
  digitalWrite(BUZZER_PIN, LOW);
}

// Funções de controle dos LEDs
void ControleRFID::controlarLEDs(bool verde, bool vermelho, bool rele, int duracao)
{
  digitalWrite(LED_ENTRAR, verde);
  digitalWrite(LED_RECUSAR, vermelho);
  digitalWrite(pinRELE, rele);

  if (verde)
  {
    ledEntrarTicker.once(duracao / 1000.0, std::bind(&ControleRFID::desligarLedEntrar, this));
  }
  if (vermelho)
  {
    ledRecusarTicker.once(duracao / 1000.0, std::bind(&ControleRFID::desligarLedRecusar, this));
  }
}

void ControleRFID::iniciarPiscarLeds()
{
  piscandoLeds = true;
  estadoLedVerde = true;
  estadoLedVermelho = false;
  piscarLeds();
}

void ControleRFID::pararPiscarLeds()
{
  piscandoLeds = false;
  digitalWrite(LED_ENTRAR, LOW);
  digitalWrite(LED_RECUSAR, LOW);
}

void ControleRFID::piscarLeds()
{
  if (piscandoLeds)
  {
    digitalWrite(LED_ENTRAR, estadoLedVerde);
    digitalWrite(LED_RECUSAR, estadoLedVermelho);
    estadoLedVerde = !estadoLedVerde;
    estadoLedVermelho = !estadoLedVermelho;
    ledPiscarTicker.once_ms(500, std::bind(&ControleRFID::piscarLeds, this));
  }
  else
  {
    digitalWrite(LED_ENTRAR, LOW);
    digitalWrite(LED_RECUSAR, LOW);
  }
}

void ControleRFID::desligarLedEntrar()
{
  digitalWrite(LED_ENTRAR, LOW);
  digitalWrite(pinRELE, LOW);
}

void ControleRFID::desligarLedRecusar()
{
  digitalWrite(LED_RECUSAR, LOW);
}

void ControleRFID::mostrarMenu()
{
  Serial.println("\n=== MENU DE CONTROLE RFID ===");
  Serial.println("1 - Cadastrar novo cartão");
  Serial.println("2 - Remover cartão");
  Serial.println("3 - Listar cartões cadastrados");
  Serial.println("4 - Apagar todos os cartões");
  Serial.print("Cartões cadastrados: ");
  Serial.println(numCartoesRegistrados);
  Serial.println("Digite uma opção:");
}

void ControleRFID::executarComando(char comando)
{
  switch (comando)
  {
  case '1':
    Serial.println("\nAproxime o cartão para cadastro...");
    modoCadastro = true;
    break;
  case '2':
    removerCartao();
    break;
  case '3':
    listarCartoes();
    mostrarMenu();
    break;
  case '4':
    apagarTodosCartoes();
    mostrarMenu();
    break;
  default:
    Serial.println("Opção inválida!");
    mostrarMenu();
    break;
  }
}

void ControleRFID::cadastrarNovoCartao()
{
  if (numCartoesRegistrados >= MAX_CARTOES)
  {
    Serial.println("Memória cheia!");
    return;
  }

  int endereco = 1 + (numCartoesRegistrados * 4);

  for (byte i = 0; i < 4; i++)
  {
    EEPROM.write(endereco + i, rfid.uid.uidByte[i]);
  }

  numCartoesRegistrados++;
  EEPROM.write(0, numCartoesRegistrados);

  if (EEPROM.commit())
  {
    Serial.println("Cartão cadastrado com sucesso!");
    Serial.print("UID: ");
    mostrarUID(rfid.uid.uidByte);
    if (mqttManager)
    {
      int uid_int = (rfid.uid.uidByte[0] << 24) |
                    (rfid.uid.uidByte[1] << 16) |
                    (rfid.uid.uidByte[2] << 8) |
                    rfid.uid.uidByte[3];
                    mqttManager->publicarCadastro("rfid", uid_int, "novo_cadastro");
    }
  }
  else
  {
    Serial.println("ERRO ao salvar na EEPROM!");
    numCartoesRegistrados--;
  }
}

void ControleRFID::removerCartao()
{
  Serial.println("\nAproxime o cartão que deseja remover...");
  while (!rfid.PICC_IsNewCardPresent() || !rfid.PICC_ReadCardSerial())
  {
    if (Serial.available() > 0)
    {
      if (Serial.read() == 'x')
      {
        Serial.println("\nOperação cancelada!");
        mostrarMenu();
        return;
      }
    }
    yield();
  }

  byte uidRemover[4];
  memcpy(uidRemover, rfid.uid.uidByte, 4);

  for (int i = 0; i < numCartoesRegistrados; i++)
  {
    byte cartaoAtual[4];
    int endereco = 1 + (i * 4);
    for (byte j = 0; j < 4; j++)
    {
      cartaoAtual[j] = EEPROM.read(endereco + j);
    }

    if (comparaUID(uidRemover, cartaoAtual))
    {
      for (int k = i; k < numCartoesRegistrados - 1; k++)
      {
        int endAtual = 1 + (k * 4);
        int endProx = 1 + ((k + 1) * 4);
        for (byte j = 0; j < 4; j++)
        {
          EEPROM.write(endAtual + j, EEPROM.read(endProx + j));
        }
      }
      numCartoesRegistrados--;
      EEPROM.write(0, numCartoesRegistrados);
      EEPROM.commit();
      Serial.println("Cartão removido com sucesso!");
      rfid.PICC_HaltA();
      rfid.PCD_StopCrypto1();
      mostrarMenu();
      if (mqttManager)
      {
        mqttManager->publicarRemocao("rfid", numCartoesRegistrados);
      }
      return;
    }
  }

  Serial.println("Cartão não encontrado!");
  rfid.PICC_HaltA();
  rfid.PCD_StopCrypto1();
  mostrarMenu();
}

void ControleRFID::listarCartoes()
{
  Serial.println("\n=== Cartões Cadastrados ===");
  for (int i = 0; i < numCartoesRegistrados; i++)
  {
    byte cartao[4];
    int endereco = 1 + (i * 4);
    for (byte j = 0; j < 4; j++)
    {
      cartao[j] = EEPROM.read(endereco + j);
    }
    Serial.print(i + 1);
    Serial.print(" - UID: ");
    mostrarUID(cartao);
  }
  Serial.println();
}

void ControleRFID::apagarTodosCartoes()
{
  numCartoesRegistrados = 0;
  EEPROM.write(0, 0);

  for (int i = 1; i <= MAX_CARTOES * 4; i++)
  {
    EEPROM.write(i, 0);
  }

  EEPROM.commit();
  Serial.println("\nTodos os cartões foram apagados!");
  if (mqttManager)
  {
    mqttManager->publicarRemocao("rfid", 0);
  }
}

void ControleRFID::mostrarUID(byte *uid)
{
  for (byte i = 0; i < 4; i++)
  {
    Serial.print(uid[i] < 0x10 ? " 0" : " ");
    Serial.print(uid[i], HEX);
  }
  Serial.println();
}

bool ControleRFID::comparaUID(byte *uid1, byte *uid2)
{
  for (byte i = 0; i < 4; i++)
  {
    if (uid1[i] != uid2[i])
      return false;
  }
  return true;
}

bool ControleRFID::verificaAcesso(byte *uid)
{
  for (int i = 0; i < numCartoesRegistrados; i++)
  {
    byte cartaoSalvo[4];
    int endereco = 1 + (i * 4);
    for (byte j = 0; j < 4; j++)
    {
      cartaoSalvo[j] = EEPROM.read(endereco + j);
    }
    if (comparaUID(uid, cartaoSalvo))
      return true;
  }
  return false;
}

void ControleRFID::fecharPorta()
{
  digitalWrite(pinRELE, LOW);
  portaAberta = false;
}

void ControleRFID::abrirPorta()
{
  if (!portaAberta)
  {
    digitalWrite(pinRELE, HIGH);
    portaAberta = true;
    portaTimer.once(3, std::bind(&ControleRFID::fecharPorta, this));
  }
}