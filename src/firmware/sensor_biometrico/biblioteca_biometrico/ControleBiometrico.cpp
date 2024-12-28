#include "ControleBiometrico.h"

// Construtor da classe ControleBiometrico
ControleBiometrico::ControleBiometrico(int rx_pin, int tx_pin)
    : fingerSerial(2), finger(&fingerSerial), lcd(0x27, 16, 2),
      LED_ENTRAR(13), LED_RECUSAR(25), CATRACA(26),
      estadoLedVerde(false), estadoLedVermelho(false), piscandoLeds(false), modo('V')
{

    // Inicializa a comunicação serial com o sensor biométrico
    fingerSerial.begin(57600, SERIAL_8N1, rx_pin, tx_pin);
    delay(100);
}

// Função para inicializar o sistema
void ControleBiometrico::inicializar()
{
    // Configura os pinos dos LEDs e da catraca como saída
    pinMode(LED_ENTRAR, OUTPUT);
    pinMode(LED_RECUSAR, OUTPUT);
    pinMode(CATRACA, OUTPUT);

    // Define o estado inicial dos LEDs e da catraca (desligados)
    digitalWrite(LED_ENTRAR, LOW);
    digitalWrite(LED_RECUSAR, LOW);
    digitalWrite(CATRACA, LOW);

    // Inicializa o display LCD e ativa a luz de fundo
    lcd.init();
    lcd.backlight();

    // Inicia a comunicação serial com o computador para mensagens de depuração
    Serial.begin(115200);
    Serial.println("\n\nSistema de Controle Biométrico");

    // Verifica a conexão com o sensor biométrico
    if (finger.verifyPassword())
    {
        Serial.println("Sensor de impressão digital encontrado!");
    }
    else
    {
        Serial.println("Sensor de impressão digital não encontrado :(");
        while (1)
            delay(1); // Loop infinito caso o sensor não seja encontrado
    }

    // Lê parâmetros do sensor biométrico e conta digitais cadastradas
    finger.getParameters();
    finger.getTemplateCount();
    Serial.print("Digitais cadastradas: ");
    Serial.println(finger.templateCount);

    // Exibe mensagem inicial no LCD
    exibirMensagemNoLCD("Instituto", "Apontar");

    // Define um temporizador para atualizar o LCD periodicamente
    lcdTicker.attach(3, atualizarLCDStatic, this);
    mostrarMenu(); // Exibe o menu de opções no Serial
}

// Função principal que roda em loop
void ControleBiometrico::loop()
{
    static bool aguardandoID = false;

    if (Serial.available() > 0)
    {
        char comando = toupper(Serial.read());

        // Limpa o buffer
        while (Serial.available())
        {
            Serial.read();
        }

        // Se recebeu um comando válido, processa
        if (comando == 'V' || comando == 'C' || comando == 'L' || comando == 'A' || comando == 'D')
        {
            modo = comando;
            if (modo == 'C')
            {
                aguardandoID = true;
            }
            processarComando(comando);
        }
    }

    // Processamento específico para cada modo
    switch (modo)
    {
    case 'V':
    { // Modo de Verificação
        if (finger.getImage() == FINGERPRINT_OK)
        {
            getFingerprintID();
            mostrarMenu();
        }
        break;
    }

    case 'C':
    { // Modo de Cadastro
        if (aguardandoID)
        {
            Serial.println("Digite o ID (1-127) para cadastrar:");
            uint8_t id = readnumber();

            if (id > 0 && id < 128)
            {
                aguardandoID = false;
                Serial.print("Cadastrando ID #");
                Serial.println(id);

                if (getFingerprintEnroll(id))
                {
                    Serial.println("Cadastro realizado com sucesso!");
                }
                else
                {
                    Serial.println("Falha no cadastro!");
                }

                modo = 'V';
                mostrarMenu();
            }
            else
            {
                Serial.println("ID inválido! Use um número entre 1 e 127.");
                aguardandoID = true;
            }
        }
        break;
    }

    case 'A':
    { // Modo Apagar Digital
        Serial.println("Digite o ID da digital para apagar (1-127):");
        uint8_t id = readnumber();
        if (id > 0 && id < 128)
        {
            apagarDigitalPorID(id);
        }
        else
        {
            Serial.println("ID inválido! Use um número entre 1 e 127.");
        }
        modo = 'V';
        mostrarMenu();
        break;
    }
    case 'L':
    { // Modo Listar
        finger.getTemplateCount();
        Serial.print("Quantidade de digitais cadastradas: ");
        Serial.println(finger.templateCount);
        mostrarMenu();
        modo = 'V';
        break;
    }

    case 'D':
    { // Modo Apagar Todas
        apagarTodasDigitais();
        modo = 'V';
        mostrarMenu();
        break;
    }
    }
}
// Processa o comando selecionado pelo usuário
void ControleBiometrico::processarComando(char comando)
{
    switch (comando)
    {
    case 'V':
        Serial.println("\nModo de Verificação ativado");
        break;
    case 'C':
        Serial.println("\nModo de Cadastro ativado");
        Serial.println("Digite o ID (1-127) para cadastrar a nova digital:");
        exibirMensagemNoLCD("Modo Cadastro", "Digite ID (1-127)");
        break;
    case 'L':
        finger.getTemplateCount();
        Serial.print("Quantidade de digitais cadastradas: ");
        Serial.println(finger.templateCount);
        mostrarMenu();
        break;
    case 'A':
        Serial.println("\nModo Apagar Digital");
        Serial.println("Digite o ID (1-127) da digital para apagar:");
        break;
    case 'D':
        apagarTodasDigitais();
        mostrarMenu();
        break;
    }
}

// Lê um número do Serial e o retorna
uint8_t ControleBiometrico::readnumber()
{
    String input = "";

    // Limpa qualquer dado residual
    while (Serial.available())
    {
        Serial.read();
    }

    // Aguarda nova entrada
    while (!Serial.available())
    {
        delay(10);
    }

    // Lê a entrada até encontrar uma nova linha
    while (Serial.available())
    {
        char c = Serial.read();
        if (c == '\n' || c == '\r')
        {
            break;
        }
        if (isdigit(c))
        {
            input += c;
            Serial.print(c); // Echo do caractere digitado
        }
        delay(10);
    }

    Serial.println(); // Nova linha após a entrada

    if (input.length() > 0)
    {
        return input.toInt();
    }
    return 0;
}

// Exibe uma mensagem no LCD
void ControleBiometrico::exibirMensagemNoLCD(const String &linha1, const String &linha2)
{
    lcd.clear();
    lcd.setCursor(0, 0);
    lcd.print(linha1);
    lcd.setCursor(0, 1);
    lcd.print(linha2);
}

// Atualiza o LCD com base no modo atual
void ControleBiometrico::atualizarLCD()
{
    switch (modo)
    {
    case 'V':
        exibirMensagemNoLCD("Aguardando", " digital");
        break;
    case 'C':
        exibirMensagemNoLCD("Modo Cadastro", "Digite ID (1-127)");
        break;
    case 'L':
        exibirMensagemNoLCD("Quantidade digitais", String(finger.templateCount));
        break;
    case 'A':
        exibirMensagemNoLCD("Modo Apagar Digital", "Digite ID (1-127)");
        break;
    case 'D':
        exibirMensagemNoLCD("Apagando digitais", "Aguarde...");
        break;
    }
}

// Identifica a digital e exibe o resultado
uint8_t ControleBiometrico::getFingerprintID()
{
    uint8_t p = finger.image2Tz();
    if (p != FINGERPRINT_OK)
    {
        switch (p)
        {
        case FINGERPRINT_IMAGEMESS:
            Serial.println("Imagem muito confusa");
            controlarLEDs(false, true, false);
            break;
        case FINGERPRINT_PACKETRECIEVEERR:
            Serial.println("Erro de comunicação");
            controlarLEDs(false, true, false);
            break;
        case FINGERPRINT_FEATUREFAIL:
        case FINGERPRINT_INVALIDIMAGE:
            Serial.println("Não foi possível encontrar características da digital");
            controlarLEDs(false, true, false);
            break;
        default:
            Serial.println("Erro desconhecido");
            controlarLEDs(false, true, false);
            break;
        }
        return p;
    }

    p = finger.fingerSearch();
    if (p == FINGERPRINT_OK)
    {
        Serial.println("Digital encontrada!");
        Serial.print("ID #");
        Serial.print(finger.fingerID);
        Serial.print(" encontrado com confiança de ");
        Serial.println(finger.confidence);
        exibirMensagemNoLCD("Digital", "ID: " + String(finger.fingerID));
        controlarLEDs(true, false, true);
        if (mqttManager)
        {
            mqttManager->publicarAcesso("biometrico", finger.fingerID, "acesso_permitido");
        }
    }
    else if (p == FINGERPRINT_NOTFOUND)
    {
        Serial.println("Digital não encontrada");
        exibirMensagemNoLCD("Digital nao", "Encontrada");
        controlarLEDs(false, true, false);
    }

    return finger.fingerID;
}

// Cadastra uma nova digital com o ID especificado
uint8_t ControleBiometrico::getFingerprintEnroll(uint8_t id)
{
    // Verifica se o ID está dentro do intervalo válido
    if (id < 1 || id > 127)
    {
        Serial.println("ID inválido! Use um ID entre 1 e 127");
        exibirMensagemNoLCD("ID invalido", "Use 1-127");
        return FINGERPRINT_BADLOCATION;
    }

    // Tenta ler o template existente para verificar se o ID já está em uso
    uint8_t p = finger.loadModel(id);
    if (p == FINGERPRINT_OK)
    {
        Serial.print("O ID #");
        Serial.print(id);
        Serial.println(" já está cadastrado.");
        exibirMensagemNoLCD("ID ja cadastrado", "");
        return FINGERPRINT_BADLOCATION;
    }

    int p_new = -1;
    Serial.print("Aguardando dedo válido para cadastrar como #");
    Serial.println(id);

    iniciarPiscarLeds();

    // Resto do código permanece igual...
    while (p_new != FINGERPRINT_OK)
    {
        p_new = finger.getImage();
        switch (p_new)
        {
        case FINGERPRINT_OK:
            Serial.println("Imagem capturada");
            break;
        case FINGERPRINT_NOFINGER:
            Serial.print(".");
            break;
        case FINGERPRINT_PACKETRECIEVEERR:
            Serial.println("Erro de comunicação");
            break;
        case FINGERPRINT_IMAGEFAIL:
            Serial.println("Erro de imagem");
            break;
        default:
            Serial.println("Erro desconhecido");
            break;
        }
    }

    p = finger.image2Tz(1);
    if (p != FINGERPRINT_OK)
    {
        Serial.println("Erro na conversão da imagem");
        pararPiscarLeds();
        return p;
    }

    Serial.println("Remova o dedo");
    delay(2000);
    p = 0;
    while (p != FINGERPRINT_NOFINGER)
    {
        p = finger.getImage();
    }

    Serial.println("Coloque o mesmo dedo novamente");
    p = -1;
    while (p != FINGERPRINT_OK)
    {
        if (modo != 'C')
        { // Se o modo mudou, sai do loop
            pararPiscarLeds();
            return FINGERPRINT_PACKETRECIEVEERR;
        }
        p = finger.getImage();
        switch (p)
        {
        case FINGERPRINT_OK:
            Serial.println("Imagem capturada");
            break;
        case FINGERPRINT_NOFINGER:
            Serial.print(".");
            break;
        case FINGERPRINT_PACKETRECIEVEERR:
            Serial.println("Erro de comunicação");
            break;
        case FINGERPRINT_IMAGEFAIL:
            Serial.println("Erro de imagem");
            break;
        default:
            Serial.println("Erro desconhecido");
            break;
        }
    }

    p = finger.image2Tz(2);
    if (p != FINGERPRINT_OK)
    {
        Serial.println("Erro na conversão da imagem");
        pararPiscarLeds();
        return p;
    }

    Serial.print("Criando modelo para #");
    Serial.println(id);
    p = finger.createModel();
    if (p != FINGERPRINT_OK)
    {
        Serial.println("Erro ao criar modelo");
        pararPiscarLeds();
        return p;
    }

    p = finger.storeModel(id);
    if (p == FINGERPRINT_OK)
    {
        Serial.println("Digital cadastrada com sucesso!");
        exibirMensagemNoLCD("Digital cadastrada", "ID: " + String(id));
        mostrarMenu(); // Exibe o menu de opções

        if (mqttManager)
        {
            mqttManager->publicarCadastro("biometrico", finger.fingerID, "criado_id");
        }
    }
    else
    {
        Serial.println("Erro ao armazenar digital");
        exibirMensagemNoLCD("Erro ao cadastrar", "");
        return p;
    }

    pararPiscarLeds();
    return true;
}

// Apaga todas as digitais após confirmação do usuário
uint8_t ControleBiometrico::apagarTodasDigitais()
{
    Serial.println("\nAviso: Isso irá apagar TODAS as impressões digitais!");
    Serial.println("Digite 'S' para confirmar ou qualquer outra tecla para cancelar");

    while (Serial.available())
    {
        Serial.read();
    }

    while (!Serial.available())
    {
        delay(10);
    }

    char confirma = Serial.read();

    if (toupper(confirma) != 'S')
    {
        Serial.println("Operação cancelada");
        return FINGERPRINT_PACKETRECIEVEERR;
    }

    Serial.println("Apagando todas as impressões digitais...");
    uint8_t p = finger.emptyDatabase();

    if (p == FINGERPRINT_OK)
    {
        Serial.println("Banco de dados apagado com sucesso!");
        exibirMensagemNoLCD("Digitais apagadas", "Banco de dados vazio");

        if (mqttManager)
        {
            mqttManager->publicarRemocao("biometrico", 0); // 0 indicates all IDs were removed
        }
    }
    else
    {
        Serial.println("Erro ao apagar banco de dados");
        Serial.print("Código de erro: ");
        Serial.println(p);
        exibirMensagemNoLCD("Erro ao apagar", "");
    }

    return p;
}

// Apaga uma digital específica pelo ID
uint8_t ControleBiometrico::apagarDigitalPorID(uint8_t id)
{
    if (id == 0)
    {
        Serial.println("ID inválido!");
        return FINGERPRINT_PACKETRECIEVEERR;
    }

    Serial.print("\nApagando ID #");
    Serial.println(id);
    uint8_t p = finger.deleteModel(id);

    if (p == FINGERPRINT_OK)
    {
        Serial.println("Digital apagada com sucesso!");
        exibirMensagemNoLCD("Digital apagada", "ID: " + String(id));

        if (mqttManager)
        {
            mqttManager->publicarRemocao("biometrico", id);
        }
    }
    else
    {
        Serial.print("Erro ao apagar digital: ");
        switch (p)
        {
        case FINGERPRINT_PACKETRECIEVEERR:
            Serial.println("Erro de comunicação");
            break;
        case FINGERPRINT_BADLOCATION:
            Serial.println("ID não encontrado");
            break;
        case FINGERPRINT_FLASHERR:
            Serial.println("Erro ao acessar memória");
            break;
        default:
            Serial.println("Erro desconhecido");
            break;
        }
        exibirMensagemNoLCD("Erro ao apagar", "");
    }

    return p;
}

// Controla os LEDs de acordo com a ação (verde para sucesso, vermelho para erro)
void ControleBiometrico::controlarLEDs(bool verde, bool vermelho, bool rele, int duracao)
{
    digitalWrite(LED_ENTRAR, verde);
    digitalWrite(LED_RECUSAR, vermelho);
    digitalWrite(CATRACA, rele);

    if (verde)
    {
        ledEntrarTicker.once(duracao / 1000.0, desligarLedEntrarStatic, this);
    }
    if (vermelho)
    {
        ledRecusarTicker.once(duracao / 1000.0, desligarLedRecusarStatic, this);
    }
}

void ControleBiometrico::iniciarPiscarLeds()
{
    piscandoLeds = true;
    estadoLedVerde = true;
    estadoLedVermelho = false;
    piscarLeds();
}

void ControleBiometrico::pararPiscarLeds()
{
    piscandoLeds = false;
    digitalWrite(LED_ENTRAR, LOW);
    digitalWrite(LED_RECUSAR, LOW);
}

void ControleBiometrico::piscarLeds()
{
    if (piscandoLeds)
    {
        digitalWrite(LED_ENTRAR, estadoLedVerde);
        digitalWrite(LED_RECUSAR, estadoLedVermelho);
        estadoLedVerde = !estadoLedVerde;
        estadoLedVermelho = !estadoLedVermelho;
        ledPiscarTicker.once_ms(500, piscarLedsStatic, this);
    }
    else
    {
        digitalWrite(LED_ENTRAR, LOW);
        digitalWrite(LED_RECUSAR, LOW);
    }
}

void ControleBiometrico::desligarLedEntrar()
{
    digitalWrite(LED_ENTRAR, LOW);
    digitalWrite(CATRACA, LOW);
}

void ControleBiometrico::desligarLedRecusar()
{
    digitalWrite(LED_RECUSAR, LOW);
}

// Exibe o menu de opções disponíveis no sistema
void ControleBiometrico::mostrarMenu()
{
    Serial.println("\n=================================");
    Serial.println("    Menu de Controle Biométrico   ");
    Serial.println("=================================");
    Serial.println("Selecione uma opção:");
    Serial.println("[V] - Modo de Verificação");
    Serial.println("[C] - Cadastrar Nova Digital");
    Serial.println("[L] - Listar Quantidade de Digitais");
    Serial.println("[A] - Apagar Digital Específica");
    Serial.println("[D] - Apagar Todas as Digitais");
    Serial.println("=================================");
    Serial.println("Modo atual: " + String(modo));
    Serial.println("Digite sua escolha...");
}

// Métodos estáticos para callbacks
void ControleBiometrico::atualizarLCDStatic(ControleBiometrico *instance)
{
    instance->atualizarLCD();
}

void ControleBiometrico::desligarLedEntrarStatic(ControleBiometrico *instance)
{
    instance->desligarLedEntrar();
}

void ControleBiometrico::desligarLedRecusarStatic(ControleBiometrico *instance)
{
    instance->desligarLedRecusar();
}

void ControleBiometrico::piscarLedsStatic(ControleBiometrico *instance)
{
    instance->piscarLeds();
}