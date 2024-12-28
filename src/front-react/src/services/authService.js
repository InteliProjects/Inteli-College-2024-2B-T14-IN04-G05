import axios from 'axios';
import Global from '../utils/Global';

// Função para logar o usuário usando axios
export const logarUsuario = async (usuario, token) => {
    try {
      const response = await axios.post(`${Global.Api}/Auth/Login`, usuario, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}` // Passa o token JWT no cabeçalho de autorização
        },
      });
      // Verifica se a resposta foi bem-sucedida
      if (!response.data.Sucesso) {
        throw new Error(response.data.message || 'Erro ao logar usuário');
      }
  
      // Retorna o token de autenticação do usuário
      return response.data; // Este é o token JWT
    } catch (error) {
      // Verifica se é um erro de rede ou um erro de processamento
      if (error.response) {
        throw new Error(error.response.data.message || 'Erro inesperado ao logar usuário');
      } else if (error.request) {
        throw new Error('Erro de conexão. Verifique sua internet e tente novamente.');
      } else {
        throw new Error(error.message || 'Erro inesperado ao logar usuário');
      }
    }
  };