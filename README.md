# Inteli - Instituto de Tecnologia e Liderança 

<p align="center">
<a href= "https://www.inteli.edu.br/"><img src="assets/inteli.png" alt="Inteli - Instituto de Tecnologia e Liderança" border="0" width=40% height=40%></a>
</p>

<br>

# ESP-42

## BiMu

## 👨‍🎓 Integrantes: 

<div align="center">
  <table>
    <tr>
      <td align="center"><a href="https://www.linkedin.com/in/davi-abreu-da-silveira/"><img style="border-radius: 10%;" src="./assets/imagens_integrantes/abreu.jpg" width="100px;" alt="Foto de Davi Abreu" /><br><sub><b>Davi Abreu</b></sub></a></td>
      <td align="center"><a href="https://www.linkedin.com/in/davi-basa/"><img style="border-radius: 10%;" src="./assets/imagens_integrantes/basa.jpg" width="100px;" alt="Foto de Davi Basã"/><br><sub><b>Davi Basã</b></sub></a></td>
      <td align="center"><a href="https://www.linkedin.com/in/felipe-zillo-72b367247/"><img style="border-radius: 10%;" src="./assets/imagens_integrantes/felipe.png" width="100px;" alt="Foto de Felipe Zillo"/><br><sub><b>Felipe Zillo</b></sub></a></td>
      <td align="center"><a href="https://www.linkedin.com/in/leonardo-ogata-983b032b5/"><img style="border-radius: 10%;" src="./assets/imagens_integrantes/leonardo.jpg" width="100px;" alt="Foto de Leonardo Ogata"/><br><sub><b>Leonardo Ogata</b></sub></a></td>
      <td align="center"><a href="https://www.linkedin.com/in/marcoruas/"><img style="border-radius: 10%;" src="./assets/imagens_integrantes/marco.jpg" width="100px;" alt="Foto de Marco Ruas"/><br><sub><b>Marco Ruas</b></sub></a></td>
      <td align="center"><a href="https://www.linkedin.com/in/rafael-barbosa-b4386b293/"><img style="border-radius: 10%;" src="./assets/imagens_integrantes/rafael.jpg" width="100px;" alt="Foto de Rafael Barbosa"/><br><sub><b>Rafael Barbosa</b></sub></a></td>
      <td align="center"><a href="https://www.linkedin.com/in/wildis-filho/"><img style="border-radius: 10%;" src="./assets/imagens_integrantes/wildisley.jpg" width="100px;" alt="Foto deWildisley Filho"/><br><sub><b>Wildisley Filho</b></sub></a></td>
    </tr>
  </table>
</div>

## 👩‍🏫 Professores:
### Orientador(a) 
- <a href="https://www.linkedin.com/in/fabiana-martins-de-oliveira-8993b0b2/">Fabiana Martins</a>
### Instrutores
- <a href="https://www.linkedin.com/in/bruna-mayer/">Bruna Mayer</a>
- <a href="https://www.linkedin.com/in/fernando-pizzo-208b526a/">Fernando Pizzo</a>
- <a href="https://www.linkedin.com/in/fillipe-resina-b2211a22/">Filipe Resina</a> 
- <a href="https://www.linkedin.com/in/filipe-gon%C3%A7alves-08a55015b/">Filipe Gonçalves</a> 
- <a href="https://www.linkedin.com/in/renato-penha/">Renato Penha</a>
- <a href="https://www.linkedin.com/in/vthayashi/">Victor Hayashi</a>

## 📜 Descrição

&nbsp;&nbsp;&nbsp;&nbsp;O projeto desenvolvido em parceria com o Instituto Apontar tem como objetivo otimizar o controle de acesso e frequência de alunos, professores e funcionários por meio da Internet das Coisas (IoT). Esta iniciativa busca não apenas melhorar a segurança e a eficiência na gestão de entradas e saídas, mas também oferecer uma solução escalável e acessível para instituições educacionais com recursos limitados.

&nbsp;&nbsp;&nbsp;&nbsp;A solução foi criada para enfrentar desafios específicos do Instituto Apontar, como a necessidade de monitorar com precisão a presença dos alunos, coordenar o fluxo de pessoas em diferentes áreas e simplificar o registro de acesso de visitantes. Esses processos eram tradicionalmente manuais e sujeitos a falhas, resultando em inconsistências e perda de tempo. O projeto visa substituir esse modelo por um sistema automatizado e integrado que garanta segurança, confiabilidade e facilidade de uso.

&nbsp;&nbsp;&nbsp;&nbsp;O sistema consiste em um protótipo físico baseado no microcontrolador ESP32, sensores biométricos para leitura de impressões digitais, leitores RFID para identificação por cartões de proximidade, display LCD com comunicação I2C para feedback visual e LEDs e buzzers para indicações sonoras. Inicialmente, o protótipo foi desenvolvido em versão offline, permitindo testes locais de autenticação e validação de acesso sem a necessidade de conexão à internet.

&nbsp;&nbsp;&nbsp;&nbsp;Na segunda fase, a solução evoluiu para uma versão online com conectividade Wi-Fi e comunicação por meio do protocolo MQTT. Essa integração permite o envio de registros de acesso em tempo real para um broker MQTT (HiveMQ), que transmite os dados para um backend em .NET e armazena-os em um banco de dados PostgreSQL. A interface web desenvolvida em React possibilita o monitoramento remoto dos acessos, exibindo informações detalhadas como nomes, cargos, datas, horários e status de autorização.

&nbsp;&nbsp;&nbsp;&nbsp;A aplicação possui funcionalidades essenciais, como cadastro de novos usuários, autenticação biométrica e por RFID, gestão de entradas e saídas, e emissão de alertas em caso de falhas ou tentativas de acesso não autorizadas. Os usuários recebem feedback instantâneo por meio de mensagens exibidas no LCD e sinais visuais e sonoros, tornando a experiência intuitiva e eficiente.

&nbsp;&nbsp;&nbsp;&nbsp;Além disso, o sistema passou por extensivos casos de teste com alunos e funcionários, identificando pontos de melhoria e garantindo sua robustez em cenários reais. Os testes validaram tanto o funcionamento do hardware quanto a estabilidade da comunicação online, evidenciando a necessidade de otimizações em situações de sobrecarga e falhas de conexão.

&nbsp;&nbsp;&nbsp;&nbsp;O impacto esperado do projeto é significativo: ele visa reduzir falhas manuais, aumentar a segurança nas instalações e facilitar a gestão de frequência escolar, contribuindo para um ambiente mais organizado e seguro para alunos e educadores. Com um sistema escalável e de baixo custo, a solução pode ser replicada em outras instituições educacionais que enfrentam desafios semelhantes.

&nbsp;&nbsp;&nbsp;&nbsp;Assim, o projeto de controle de acesso e frequência IoT representa uma inovação importante para o Instituto Apontar, combinando tecnologia acessível e design centrado no usuário para otimizar processos administrativos e promover um ambiente educacional mais eficiente e seguro.

[Link para o vídeo de demonstração do funcionamento do projeto]()

## 📁 Estrutura de pastas

Dentre os arquivos e pastas presentes na raiz do projeto, definem-se:

- <b>assets</b>: aqui estão os arquivos relacionados a parte gráfica do projeto, ou seja, as imagens e vídeos que os representam (O logo do grupo pode ser adicionado nesta pasta).

- <b>document</b>: aqui estão todos os documentos do projeto, incluindo o manual de instruções (se aplicável). Há também uma pasta denominada <b>outros</b> onde estão presentes outros documentos complementares.

- <b>src</b>: Todo o código fonte criado para o desenvolvimento do projeto, incluindo firmware, notebooks, backend e frontend, se aplicáveis.

- <b>README.md</b>: arquivo que serve como guia e explicação geral sobre o projeto (o mesmo que você está lendo agora).

## 🔧 Instalação

&nbsp;&nbsp;&nbsp;&nbsp;Este guia foi desenvolvido para facilitar a configuração e execução do projeto ESP-42, oferecendo orientações claras tanto para desenvolvedores quanto para colaboradores. Nele, você encontrará detalhes sobre os pré-requisitos necessários, como ferramentas, linguagens de programação e serviços em nuvem, além de etapas práticas para instalar e rodar o backend, frontend e o firmware do dispositivo. Utilize este material para preparar o ambiente de desenvolvimento e explorar as funcionalidades de maneira eficiente.

### 📋 **Pré-requisitos**

Para garantir a correta execução do projeto ESP-42, certifique-se de atender aos seguintes requisitos de ambiente:

- **Arduino IDE**: Versão 2.1.1 ou superior. [Baixe aqui](https://www.arduino.cc/en/software).
   - **Bibliotecas necessárias:**
   - `Adafruit_Fingerprint`
   - `LiquidCrystal_I2C`
   - `MQTTManager`
   - `WiFi`
   - `WiFiClientSecure`
   - `PubSubClient`
   - `ArduinoJson`
   - `EEPROM`
   - `SPI`
   - `MFRC522`
- **Visual Studio**: Versão Community 2022. [Baixe aqui](https://visualstudio.microsoft.com/pt-br/downloads/).
   - **Bibliotecas necessárias:**
   - `Automapper`
   - `Microsoft.EntityFrameworkCore.Design`
   - `Npgsql.EntityFrameworkCore.PostgreSQL`
   - `Pomelo.EntityFrameworkCore.MySql`
- **Node.js**: Versão 16.x LTS. [Baixe aqui](https://nodejs.org/).
- **Git**: Para controle de versão. [Baixe aqui](https://git-scm.com/).
- **Render**: Para hospedagem do banco de dados PostgreSQL. [Acesse aqui](https://render.com/).
- **HiveMQ**: Para o broker MQTT. [Acesse aqui](https://www.hivemq.com/).
- **Ferramentas de linha de comando**: 
  - **Windows**: PowerShell.
  - **macOS/Linux**: Terminal padrão.

### 📜 **Manual de Instruções**

&nbsp;&nbsp;&nbsp;&nbsp;O Manual de Instruções do projeto é um recurso indispensável para compreender todos os aspectos do nosso sistema. Projetado tanto para desenvolvedores quanto para usuários finais, este manual fornece informações detalhadas sobre a configuração, funcionamento e manutenção do sistema.

&nbsp;&nbsp;&nbsp;&nbsp;Informações mais detalhadas sobre a instalação completa do projeto podem ser encontradas nele.

[**Clique aqui para acessar o Manual de Instruções do projeto**](https://docs.google.com/document/d/1zzpDwJzoZm2AtAILqwlDfhTMf2NrttlS).

## 🗃 Histórico de lançamentos

### 0.5.0 - 19/12/2024  
- Protótipo Final do Projeto  
- Manual de instruções

### 0.4.0 - 06/12/2024  
- Protótipo Físico Online  
- Fluxos de Funcionamento com UML  

### 0.3.0 - 22/11/2024  
- Protótipo Físico Offline  
- Testes de Guerrilha  

### 0.2.0 - 08/11/2024  
- Código do Firmware ESP32  
- Implementação de Sensor Biométrico e RFID  

### 0.1.0 - 25/10/2024  
- Requisitos Funcionais e Não Funcionais  
- Estrutura Inicial do Projeto  
- Análise de negócios


## 📋 Licença/License

<img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/cc.svg?ref=chooser-v1"><img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/by.svg?ref=chooser-v1"><p xmlns:cc="http://creativecommons.org/ns#" xmlns:dct="http://purl.org/dc/terms/"><a property="dct:title" rel="cc:attributionURL" href="https://github.dev/Intelihub/Template_M3">ESP-42</a> by <a rel="cc:attributionURL dct:creator" property="cc:attributionName" href="https://www.inteli.edu.br/">Inteli</a>, <a href="https://github.com/daviiabreu">Davi Abreu<a>, <a href="https://www.linkedin.com/in/davi-basa/">Davi Basã</a>, <a href="https://www.linkedin.com/in/felipe-zillo-72b367247/">Filipe Zillo</a>, <a href="https://www.linkedin.com/in/leonardo-ogata-983b032b5/">Leonardo Ogata</a>, <a href="https://www.linkedin.com/in/marcoruas/">Marco Ruas</a>, <a href="https://www.linkedin.com/in/rafael-barbosa-b4386b293/">Rafael Barbosa</a>, <a href="https://www.linkedin.com/in/wildis-filho/">Wildisley Filho</a>