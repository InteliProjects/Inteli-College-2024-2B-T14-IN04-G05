// Importa as bibliotecas necessárias e componentes customizados
import React, { useEffect, useState } from "react";
import Sidebar from "../components/sidebar/sidebar"; // Componente da barra lateral
import SearchBar from "../components/searchbar/searchbar"; // Componente da barra de busca
import Table from "../components/Table/accessTable"; // Componente da tabela de acessos

// Define o componente EntradasESaidas
const EntradasESaidas = () => {
    // Estado para armazenar os dados da tabela
    const [data, setData] = useState([]);

    // Função assíncrona para buscar o histórico de acessos
    const fetchHistoricoAcesso = async () => {
        try {
            // Faz uma requisição para a API
            const response = await fetch("https://inteli.azurewebsites.net/api/HistoricoAcesso");
            const result = await response.json();

            // Formata os dados recebidos para o formato necessário para exibição
            const dadosFormatados = result.dados.map((item) => ({
                nome: item.pessoa.nome, // Nome da pessoa
                cargo: item.pessoa.tipoPessoa.tipo_Pessoa_Desc, // Cargo ou tipo da pessoa
                dataHora: new Date(item.data).toLocaleString(), // Data e hora formatadas
                diaSemana: obterDiaSemana(item.data), // Dia da semana calculado
                porta: item.area.area, // Identificação da porta
                entradaSaida: item.entrada_Saida, // Status de entrada ou saída
            }));

            setData(dadosFormatados); // Atualiza o estado com os dados formatados
        } catch (error) {
            // Trata erros de requisição
            console.error("Erro ao buscar dados:", error);
        }
    };

    // Função para obter o dia da semana a partir de uma data
    const obterDiaSemana = (dataString) => {
        const diasSemana = [
            "Domingo",
            "Segunda-feira",
            "Terça-feira",
            "Quarta-feira",
            "Quinta-feira",
            "Sexta-feira",
            "Sábado",
        ];
        return diasSemana[new Date(dataString).getDay()]; // Retorna o nome do dia correspondente
    };

    // Hook useEffect para buscar os dados ao carregar o componente
    useEffect(() => {
        fetchHistoricoAcesso();
    }, []); // Array de dependências vazio significa que executa apenas uma vez ao montar

    return (
        // Layout principal
        <div className="flex">
            {/* Componente da barra lateral */}
            <Sidebar />
            {/* Conteúdo principal */}
            <div className="ml-16 p-6 w-full">
                {/* Título da página */}
                <h1 className="text-2xl font-bold mb-4">Entradas e Saídas</h1>
                {/* Barra de busca com a função fetchHistoricoAcesso associada */}
                <SearchBar onSearch={fetchHistoricoAcesso} />
                {/* Tabela que recebe os dados formatados como propriedade */}
                <Table data={data} />
            </div>
        </div>
    );
};

// Exporta o componente para ser usado em outros arquivos
export default EntradasESaidas;
