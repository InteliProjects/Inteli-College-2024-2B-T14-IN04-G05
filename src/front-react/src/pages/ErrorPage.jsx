import React from 'react';
import { Link } from 'react-router-dom';

const NotFound = () => {
  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="bg-red-800 p-8 rounded-lg shadow-lg text-center">
        <h1 className="text-4xl font-bold text-white mb-4">404</h1>
        <h2 className="text-2xl text-white mb-6">Página Não Encontrada</h2>
        <p className="text-white mb-6">
          Desculpe, a página que você está procurando não existe.
        </p>
        <Link 
          to="/" 
          className="bg-white text-red-800 py-2 px-4 rounded-md hover:bg-red-700 hover:text-white transition-colors duration-300"
        >
          Voltar para Página Inicial
        </Link>
      </div>
    </div>
  );
};

export default NotFound;