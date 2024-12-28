// Importa o React
import React from "react";

// Define o componente AccessTable, que recebe a propriedade 'data' contendo os dados a serem exibidos na tabela
const AccessTable = ({ data }) => {
    return (
        // Wrapper com overflow-x-auto para permitir rolagem horizontal em telas menores
        <div className="overflow-x-auto">
            {/* Tabela estilizada com Tailwind CSS */}
            <table className="min-w-full bg-white border border-gray-200 shadow-md rounded-md font-color-red">
                {/* Cabeçalho da tabela */}
                <thead>
                    <tr className="bg-gray-100">
                        {/* Define os cabeçalhos das colunas com espaçamento, alinhamento e cores */}
                        <th className="px-6 py-3 text-left text-red-600">Nome</th>
                        <th className="px-6 py-3 text-left text-gray-600">Cargo</th>
                        <th className="px-6 py-3 text-left text-gray-600">Data/Hora</th>
                        <th className="px-6 py-3 text-left text-gray-600">Dia da Semana</th>
                        <th className="px-6 py-3 text-left text-gray-600">Porta</th>
                        <th className="px-6 py-3 text-left text-gray-600">Entrada/Saída</th>
                    </tr>
                </thead>
                {/* Corpo da tabela */}
                <tbody>
                    {/* Itera sobre os dados fornecidos e cria uma linha para cada item */}
                    {data.map((item, index) => (
                        <tr
                            key={index} // Define uma chave única para cada linha
                            className={`${index % 2 === 0 ? "bg-gray-50" : "bg-white"} hover:bg-gray-100`}
                            // Alterna as cores das linhas para efeito de "zebra" (bg-gray-50 e bg-white)
                            // Adiciona um efeito hover que muda a cor do fundo para bg-gray-100
                        >
                            {/* Exibe os dados em células da tabela */}
                            <td className="px-6 py-3">{item.nome}</td>
                            <td className="px-6 py-3">{item.cargo}</td>
                            <td className="px-6 py-3">{item.dataHora}</td>
                            <td className="px-6 py-3">{item.diaSemana}</td>
                            <td className="px-6 py-3">{item.porta}</td>
                            <td className="px-6 py-3">{item.entradaSaida}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

// Exporta o componente para ser usado em outros lugares
export default AccessTable;
