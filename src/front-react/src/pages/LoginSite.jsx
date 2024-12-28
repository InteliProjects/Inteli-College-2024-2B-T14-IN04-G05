import React, { useState, useEffect } from 'react';
import { logarUsuario } from "../services/authService"; // Importando os serviços
import { Link, useNavigate } from "react-router-dom";

const Login = () => {
  const [username, setUsername] = useState(''); // Estado para o usuário
  const [password, setPassword] = useState(''); // Estado para a senha
  const [errorMessage, setErrorMessage] = useState(''); // Mensagem de erro para validação
  const navigate = useNavigate();

  useEffect(() => {
    // Verificar se o JWT está salvo no localStorage
    const token = localStorage.getItem('jwt');
    console.log("token", token);
    if (token) {
      // Redirecionar para a página principal (ou outra página)
      window.location.href = '/';
    }
  }, []);

  const handlePasswordChange = (e) => {
    const newPassword = e.target.value;
    setPassword(newPassword);

    // // Validação de senha com regex
    // const regex =
    //   /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/;
    // if (!regex.test(newPassword)) {
    //   setErrorMessage(
    //     'A senha deve ter no mínimo 6 caracteres, incluindo letra maiúscula, letra minúscula, número e caractere especial.'
    //   );
    // } else {
    //   setErrorMessage('');
    // }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Verificar se há erros antes de submeter
    if (errorMessage) {
      alert('Por favor, corrija os erros antes de continuar.');
      return;
    }

    try {
      // Autentica o usuário com email e senha
      const resultadoUsuario = await logarUsuario({ 'user': username,  'password': password });
      console.log(resultadoUsuario.Dados)
      localStorage.setItem('jwt', resultadoUsuario.Dados);
      alert('Login realizado com sucesso!');
      navigate("/");
    } catch (error) {
      // Verifica se é um erro de rede
      if (error.name === "TypeError") {
        console.log(error);
      } else {
        console.log(error);
      }
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="bg-red-800 p-8 rounded-lg shadow-lg w-96">
        <h1 className="text-center text-2xl font-bold mb-6 text-white">Bem vindo!</h1>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label htmlFor="username" className="block text-sm font-medium text-white">
              Usuário
            </label>
            <input
              type="text"
              id="username"
              className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-gray-500 focus:border-gray-500"
              placeholder="Digite seu usuário"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          <div className="mb-6">
            <label htmlFor="password" className="block text-sm font-medium text-white">
              Senha
            </label>
            <input
              type="password"
              id="password"
              className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-gray-500 focus:border-gray-500"
              placeholder="Digite sua senha"
              value={password}
              onChange={handlePasswordChange}
              required
            />
            {errorMessage && (
              <p className="mt-1 text-sm text-yellow-300">{errorMessage}</p>
            )}
          </div>
          <button
            type="submit"
            className="w-full bg-white text-red-800 py-2 rounded-md hover:bg-red-700 hover:text-white transition-colors duration-300"
          >
            Login
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;
