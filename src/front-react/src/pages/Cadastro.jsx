import React, { useState, useEffect } from "react";
import genericService from "../services/genericoService";

const Cadastro = () => {
  const [nomeSelecionado, setNomeSelecionado] = useState({}); // Nome selecionado
  const [nomes, setNomes] = useState([]); // Lista de nomes
  const [tipoPessoa, setTipoPessoa] = useState(""); // Tipo de pessoa
  const [senha, setSenha] = useState(""); // Senha
  const [confirmarSenha, setConfirmarSenha] = useState(""); // Confirmar senha
  const [erroSenha, setErroSenha] = useState(""); // Erro de validação da senha

  useEffect(() => {
    // Verificar se o JWT está salvo no localStorage
    const token = localStorage.getItem("jwt");
    if (token) {
      window.location.href = "/login";
    }

    // Buscar nomes da API
    const fetchNomes = async () => {
      try {
        const response = await genericService.getRequest("/Pessoas/GetAllNomes"); // Ajuste o endpoint
        console.log(response) 
        if (response.sucesso) {
          setNomes(response.dados); // Assuma que a API retorna um array de objetos com "id" e "nome"
        } else {
          console.error("Erro ao buscar nomes:", response.mensagem);
        }
      } catch (error) {
        console.error("Erro ao buscar nomes:", error.mensagem);
      }
    };

    fetchNomes();
  }, []);

  const handleNomeChange = (e) => {
    console.log(e.target);
    const selectedOption = e.target.selectedOptions[0]; // Obter a opção selecionada
    const id = selectedOption.dataset.id; // Obter o atributo data-id
    const nome = selectedOption.value; // Obter o valor do option
    setNomeSelecionado({ id: id, nome: nome }); // Atualizar o estado
  };

  const handleTipoChange = (e) => {
    setTipoPessoa(e.target.value);
  };

  const handleSenhaChange = (e) => {
    const novaSenha = e.target.value;
    setSenha(novaSenha);

    const regex =
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    if (!regex.test(novaSenha)) {
      setErroSenha(
        "A senha deve ter no mínimo 8 caracteres, incluindo letra maiúscula, letra minúscula, número e caractere especial."
      );
    } else {
      setErroSenha("");
    }
  };

  const handleConfirmarSenhaChange = (e) => {
    setConfirmarSenha(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // if (senha !== confirmarSenha) {
    //   alert("As senhas não coincidem!");
    //   return;
    // }

    // if (erroSenha) {
    //   alert("Por favor, corrija os erros antes de continuar.");
    //   return;
    // }

    const payload = {
      FuncId: nomeSelecionado.id, // Pega o ID do nome selecionado
      Usuario: nomeSelecionado.nome, // Define o tipo de pessoa como usuário
      Password: senha,
    };

    console.log(payload);
    try {
      const response = await genericService.postRequest("/Auth/Registrar", payload); // Ajuste o endpoint
      if (response.sucesso) {
        alert("Cadastro realizado com sucesso!");
      } else {
        alert(`Erro no cadastro: ${response.mensagem}`);
      }
    } catch (error) {
      console.error("Erro ao realizar cadastro:", error.mensagem);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="bg-red-800 p-8 rounded-lg shadow-lg w-96">
        <h1 className="text-center text-2xl font-bold mb-6 text-white">
          Cadastro
        </h1>
        <form onSubmit={handleSubmit}>
          {/* Dropdown Nome */}
          <div className="mb-4">
            <label htmlFor="nome" className="block text-sm font-medium text-white">
              Nome
            </label>
            <select
              id="nome"
              className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-gray-500 focus:border-gray-500"
              onChange={handleNomeChange}
            >
              <option value="">Selecione um nome</option>
              {nomes.map((nome) => (
                <option data-id={nome.id} key={nome.id} value={nome.nome}>
                  {nome.nome}
                </option>
              ))}
            </select>
          </div>

          {/* Dropdown Tipo de Pessoa */}
          <div className="mb-4">
            <label htmlFor="pessoa" className="block text-sm font-medium text-white">
              Tipo de Pessoa
            </label>
            <select
              id="pessoa"
              className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-gray-500 focus:border-gray-500"
              onChange={handleTipoChange}
            >
              <option value="">Selecione</option>
              <option value="Funcionário">Funcionário</option>
              <option value="Professor">Professor</option>
            </select>
          </div>

          {/* Campo Senha */}
          <div className="mb-4">
            <label htmlFor="senha" className="block text-sm font-medium text-white">
              Senha
            </label>
            <input
              type="password"
              id="senha"
              className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-gray-500 focus:border-gray-500"
              placeholder="Digite sua senha"
              value={senha}
              onChange={handleSenhaChange}
            />
            {erroSenha && (
              <p className="mt-1 text-sm text-yellow-300">{erroSenha}</p>
            )}
          </div>

          {/* Campo Confirmar Senha */}
          <div className="mb-6">
            <label htmlFor="confirmarSenha" className="block text-sm font-medium text-white">
              Confirmar Senha
            </label>
            <input
              type="password"
              id="confirmarSenha"
              className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-gray-500 focus:border-gray-500"
              placeholder="Confirme sua senha"
              value={confirmarSenha}
              onChange={handleConfirmarSenhaChange}
            />
          </div>

          {/* Botão de Cadastro */}
          <button
            type="submit"
            className="w-full bg-white text-red-800 py-2 rounded-md hover:bg-red-700 hover:text-white transition-colors duration-300"
          >
            Cadastrar
          </button>
        </form>
      </div>
    </div>
  );
};

export default Cadastro;
