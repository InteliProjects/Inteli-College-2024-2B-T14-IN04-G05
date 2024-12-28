// Importa o React e o hook useState para gerenciar o estado do componente
import React, { useState } from "react";

// Define o componente SearchBar, que recebe uma função 'onSearch' como propriedade
const SearchBar = ({ onSearch }) => {
    // Cria um estado chamado 'query' com valor inicial vazio e uma função para atualizá-lo
    const [query, setQuery] = useState("");

    // Função chamada quando o botão "Buscar" é clicado
    // Chama a função 'onSearch' recebida como propriedade, passando o valor de 'query'
    const handleSearch = () => {
        onSearch(query);
    };

    return (
        // Container principal da barra de busca com classes de estilo Tailwind CSS
        <div className="flex items-center justify-between gap-4 p-4 bg-gray-100 rounded-md shadow-sm">
            {/* Campo de entrada de texto para digitar a busca */}
            <input
                type="text" // Define o tipo como texto
                value={query} // Vincula o valor do input ao estado 'query'
                onChange={(e) => setQuery(e.target.value)} // Atualiza 'query' sempre que o texto muda
                placeholder="Digite sua busca..." // Texto de sugestão exibido no campo
                className="flex-grow px-4 py-2 border rounded-md outline-none focus:ring-2 focus:ring-blue-400" // Estilização usando Tailwind CSS
            />
            {/* Botão para realizar a busca */}
            <button
                onClick={handleSearch} // Define a ação a ser executada ao clicar no botão
                className="px-4 py-2 text-white bg-red-500 rounded-md hover:bg-red-600" // Estilização usando Tailwind CSS
            >
                Buscar
            </button>
        </div>
    );
};

// Exporta o componente para ser utilizado em outros arquivos
export default SearchBar;
