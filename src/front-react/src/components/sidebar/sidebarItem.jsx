// Importa o React e o componente Link do React Router para navegação
import React from "react";
import { Link } from "react-router-dom";

// Define o componente SidebarItem que recebe propriedades: icon, label e link
const SidebarItem = ({ icon, label, link }) => {
    return (
        // Cria um link de navegação estilizado usando Tailwind CSS
        <Link
            to={link} // Define o destino do link
            className="flex items-center gap-4 px-4 py-3 text-gray-300 hover:bg-gray-700 hover:text-white transition"
            // Estilização:
            // - Flex para alinhamento horizontal
            // - Espaçamento entre ícone e rótulo (gap-4)
            // - Padding interno (px-4 py-3)
            // - Cor de texto padrão (text-gray-300)
            // - Efeitos visuais ao passar o mouse: fundo cinza e texto branco (hover:bg-gray-700 hover:text-white)
            // - Transição suave entre os estados (transition)
        >
            {/* Exibe o ícone fornecido na propriedade icon */}
            <span className="text-xl">{icon}</span>
            {/* Exibe o rótulo fornecido na propriedade label */}
            <span className="text-lg">{label}</span>
        </Link>
    );
};

// Exporta o componente para ser usado em outros lugares
export default SidebarItem;
