<<<<<<< HEAD
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import './index.css'
import App from './App.jsx'
import Login from './pages/LoginSite.jsx'
import Cadastro from './pages/Cadastro.jsx'
import Error from './pages/ErrorPage.jsx'
import EntradasESaidas from './pages/entradas-e-saidas.jsx'


const isAuthenticated = !!localStorage.getItem("jwt");

=======
// Importa `StrictMode` para destacar possíveis problemas e boas práticas no código React.
import { StrictMode } from 'react';

// Importa `createRoot` para criar a raiz da aplicação React no novo modelo de renderização.
import { createRoot } from 'react-dom/client';

// Importa componentes de roteamento do `react-router-dom` para gerenciar as rotas da aplicação.
import { BrowserRouter, Routes, Route } from 'react-router-dom';

// Importa o arquivo de estilo CSS global para aplicar estilos à aplicação.
import './index.css';

// Importa o componente principal `App`.
import App from './App.jsx';

// Importa o componente `Nav` para navegação.
import Nav from './pages/Nav.jsx';

// Importa o componente `EntradasESaidas` para a página de entradas e saídas.
import EntradasESaidas from './pages/entradas-e-saidas.jsx';

// Cria a raiz da aplicação e renderiza os componentes dentro dela.
>>>>>>> e26be83c50963d5026d4cae9913cf511e6c03716
createRoot(document.getElementById('root')).render(
  // Envolve a aplicação em `StrictMode` para alertar sobre possíveis problemas durante o desenvolvimento.
  <StrictMode>
    {/* `BrowserRouter` é o roteador principal que permite a navegação baseada em URLs. */}
    <BrowserRouter>
      {/* Define as rotas da aplicação usando o componente `Routes`. */}
      <Routes>
<<<<<<< HEAD
        <Route path='/' element={<App />} />
        <Route path='/entradas-e-saidas' element={< EntradasESaidas />} />
        <Route path='/login' element={<Login />} />
        <Route path='/cadastro' element={<Cadastro />} />
        <Route path='*' element={<Error />} />

        {isAuthenticated ? (
          <Route path="/" element={<App />} />
        ) : (
          <Route path="/login" element={<Login />} />
        )}
=======
        {/* Rota para a página inicial que renderiza o componente `App`. */}
        <Route path='/' element={<App />} />

        {/* Rota para a página de configurações que também renderiza o componente `App`. */}
        <Route path='/configuracoes' element={<App />} />

        {/* Rota para a página de entradas e saídas que renderiza o componente `EntradasESaidas`. */}
        <Route path='/entradas-e-saidas' element={<EntradasESaidas />} />

        {/* Rota para a navegação que renderiza o componente `Nav`. */}
        <Route path='/nav' element={<Nav />} />

        {/* Repetição da mesma rota '/nav' que renderiza o componente `Nav`. Pode ser removida ou corrigida. */}
        <Route path='/nav' element={<Nav />} />
>>>>>>> e26be83c50963d5026d4cae9913cf511e6c03716
      </Routes>
    </BrowserRouter>
  </StrictMode>
);
