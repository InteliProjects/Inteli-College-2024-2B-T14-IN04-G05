// Importa a biblioteca React para utilizar componentes.
import React from 'react';

// Importa o componente `EntradasESaidas` da pasta `pages`.
// Este componente será renderizado pela aplicação principal.
import EntradasESaidas from './pages/entradas-e-saidas';

// Importa o arquivo de estilo CSS global para aplicar estilos à aplicação.
import './index.css';

// Define o componente funcional `App`, que serve como o componente principal da aplicação.
const App = () => {
    // Renderiza o componente `EntradasESaidas`.
    return <EntradasESaidas />;
};

// Exporta o componente `App` como padrão para ser usado em outros arquivos (por exemplo, `index.js`).
export default App;
