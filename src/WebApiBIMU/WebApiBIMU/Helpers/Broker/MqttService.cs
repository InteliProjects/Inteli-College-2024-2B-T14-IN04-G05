using HiveMQtt.Client.Options;
using HiveMQtt.Client;
using HiveMQtt.MQTT5.ReasonCodes;
using HiveMQtt.MQTT5.Types;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
 

public static class MqttService
{
    static HiveMQClientOptions options = new HiveMQClientOptions
    {
        Host = "706f3a6885be461ea18a14eeca8916ee.s1.eu.hivemq.cloud",
        Port = 8883,
        UseTLS = true,
        UserName = "admin",
        Password = "Admin1234",
    };
    public static readonly HiveMQClient _client = new HiveMQClient(options);

    private static IServiceProvider _serviceProvider;  // UnitOfWork para acessar serviços e gerenciar transações.

    
    //public MqttService()
    //{


    //    _client = new HiveMQClient(options);
    //    Connect().Wait();
    //    Task.Run(() => Connect()); // Garante que a conexão seja feita em uma tarefa separada

    //}

    // Método para configurar o ServiceProvider
    public static void Configure(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Task.Run(() => Connect()); // Garante que a conexão seja feita em uma tarefa separada
    }


    public static async Task Connect()
    {

        Console.WriteLine($"Connecting to {_client.Options.Host} on port {_client.Options.Port} ...");
        try
        {
            var connectResult = await _client.ConnectAsync().ConfigureAwait(false);
            if (connectResult.ReasonCode == ConnAckReasonCode.Success)
            {
                Console.WriteLine("Connect successful");
                await SubscribeAsync("instituto/acesso/biometrico/#");
                _client.OnMessageReceived += OnMessageReceived; // Evento para capturar mensagens recebidas

            }
            else
            {
                Console.WriteLine($"Connect failed: {connectResult}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to the MQTT Broker: {ex.Message}");
        }
    }

    private static async void OnMessageReceived(object sender, HiveMQtt.Client.Events.OnMessageReceivedEventArgs args)
    {
        string receivedMessage = args.PublishMessage.PayloadAsString;
        Console.WriteLine($"Message received: {receivedMessage}");

        if (_serviceProvider == null)
        {
            Console.WriteLine("ServiceProvider não configurado.");
            return;
        }

        try
        {
            // Deserializar a mensagem recebida para um objeto C#
            var jsonData = JsonSerializer.Deserialize<AcessoDto>(receivedMessage);
            if (jsonData == null)
            {
                Console.WriteLine("Mensagem inválida");
                return;
            }

            // Criar um escopo para o UnitOfWork
            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var pessoa = await unitOfWork.pessoas.GetSingle(jsonData.Id);
                if (pessoa == null)
                {
                    Console.WriteLine("Pessoa não encontrada");
                    return;
                }
                var historicos = await unitOfWork.historicoAcesso.QueryAsync(
                    filter: h => h.Id_Pessoa == jsonData.Id,
                    orderBy: query => query.OrderByDescending(h => h.Data).ThenByDescending(h => h.Horario)
                );

                var ultimoHistorico = historicos.FirstOrDefault();

                // Definir se a próxima ação será de entrada ou saída
                bool entradaSaida = true; // Default: Entrada
                if (ultimoHistorico != null)
                {
                    entradaSaida = !ultimoHistorico.Entrada_Saida; // Alterna a ação (se foi entrada, agora será saída, e vice-versa)
                }

                // Registrar o acesso no histórico
                var historicoAcesso = new HistoricoAcesso
                {
                    Id_Pessoa = jsonData.Id,
                    Id_Area = 1,
                    Data = DateTime.UtcNow,
                    Horario = DateTime.Now.TimeOfDay,
                    Entrada_Saida = entradaSaida
                };

                await unitOfWork.historicoAcesso.Add(historicoAcesso);

                Console.WriteLine("Acesso registrado com sucesso");
            }
        }
        catch (JsonException)
        {
            Console.WriteLine("Erro ao deserializar JSON");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar o acesso no banco de dados: {ex.Message}");
        }
    }
    public static async Task PublishMessageAsync(string topic, string message)
    {
        var publishResult = await _client.PublishAsync(topic, message, QualityOfService.AtLeastOnceDelivery).ConfigureAwait(false);
        Console.WriteLine($"Message published to topic {topic}: {message}");
    }

    public static async Task SubscribeAsync(string topic)
    {
        await _client.SubscribeAsync(topic).ConfigureAwait(false);
        Console.WriteLine($"Subscribed to topic: {topic}");
    }
}

// DTO para deserialização
public class AcessoDto
{
    public string Tipo { get; set; }
    public int Id { get; set; }
    public string Status { get; set; }
}
