// Importa o React e o componente SidebarItem
import React from "react";
import SidebarItem from "./sidebarItem";

// Importa ícones de bibliotecas externas
import { DiAptana } from "react-icons/di";
import { BsFillDoorOpenFill } from "react-icons/bs";
import { BsFillHouseDoorFill } from "react-icons/bs";

// Define o componente Sidebar
const Sidebar = () => {
    // Array contendo os itens do menu lateral com ID, rótulo, ícone e link
    const menuItems = [
        { id: 1, label: "Dashboard", icon: <BsFillHouseDoorFill/>, link: "/" }, // Item para o Dashboard
        { id: 2, label: "Entradas e Saídas", icon: <BsFillDoorOpenFill/>, link: "/entradas-e-saidas" }, // Item para Entradas e Saídas
        { id: 3, label: "Configurações", icon: <DiAptana />, link: "/configuracoes" }, // Item para Configurações
    ];

    return (
        // Container principal da barra lateral com altura total, largura fixa e estilo Tailwind CSS
        <div className="h-screen w-64 bg-red-800 text-white shadow-md">
            {/* Título do aplicativo exibido no topo da barra lateral */}
            <h1 className="p-4 text-2xl font-bold text-center border-b border-red-700">Minha App</h1>
            {/* Navegação contendo os itens do menu */}
            <nav className="mt-4">
                {/* Mapeia o array menuItems para renderizar cada item como um componente SidebarItem */}
                {menuItems.map((item) => (
                    <SidebarItem 
                        key={item.id} // Chave única para o React
                        icon={item.icon} // Ícone do item
                        label={item.label} // Rótulo do item
                        link={item.link} // Link associado ao item
                    />
                ))}
            </nav>
        </div>
    );
};

// Exporta o componente Sidebar para uso em outros arquivos
export default Sidebar;
