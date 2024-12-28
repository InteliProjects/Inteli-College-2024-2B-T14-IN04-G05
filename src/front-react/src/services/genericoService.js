// Importa a biblioteca Axios para fazer requisições HTTP.
import axios from "axios";

// Importa o objeto `Global`, que contém configurações globais, como a URL da API.
import Global from "../utils/Global";

// Define a classe `GenericService` para realizar operações CRUD (GET, POST, DELETE, PUT) com a API.
class GenericService {
    // O construtor recebe uma `baseURL` e um `timeout` padrão de 50000 milissegundos.
    constructor(baseURL, timeout = 50000) {
        // Cria uma instância Axios com a URL base e o tempo limite configurados.
        this.client = axios.create({
            baseURL,
            timeout,
        });
    }

    // Método para realizar requisições GET.
    async getRequest(endpoint, params = {}, headersParams = {}, api = Global.Api) {
        var response;
        try {
            // Adiciona cabeçalhos à requisição utilizando o método `addHeaders` da classe `Global`.
            await Global.addHeaders(this.client, window.localStorage, headersParams);
            
            // Faz a requisição GET com os parâmetros fornecidos.
            response = await this.client.get(api + endpoint, { params });
            
            // Retorna os dados da resposta em caso de sucesso.
            return response.data;
        } catch (error) {
            // Loga o erro no console.
            console.log(error);
            
            // Retorna um objeto de erro estruturado.
            response = {
                data: error,
                statusCode: error.status,
                success: false,
                message: error.message,
            }
            return response;
        }
    }

    // Método para realizar requisições POST.
    async postRequest(endpoint, data, headersParams = {}, api = Global.Api) {
        var response;
        try {
            // Adiciona cabeçalhos à requisição utilizando o método `addHeaders` da classe `Global`.
            await Global.addHeaders(this.client, window.localStorage, headersParams);
            
            // Faz a requisição POST com os dados fornecidos.
            response = await this.client.post(api + endpoint, data);
            
            // Retorna os dados da resposta em caso de sucesso.
            return response.data;
        } catch (error) {
            // Loga o erro no console.
            console.log(error);
            
            // Se o erro for 404, retorna uma mensagem específica.
            if (error.status == 404) {
                response = {
                    data: error,
                    statusCode: error.status,
                    success: false,
                    message: "Nenhuma informação encontrada",
                }
            } else {
                // Retorna um objeto de erro padrão para outros tipos de erro.
                response = {
                    data: error,
                    statusCode: error.status,
                    success: false,
                    message: error.message,
                }
            }
            return response;
        }
    }

    // Método para realizar requisições DELETE.
    async deleteRequest(endpoint, headersParams = {}, api = Global.Api) {
        var response;
        try {
            // Adiciona cabeçalhos à requisição utilizando o método `addHeaders` da classe `Global`.
            await Global.addHeaders(this.client, window.localStorage, headersParams);
            
            // Faz a requisição DELETE.
            response = await this.client.delete(api + endpoint);
            
            // Retorna os dados da resposta em caso de sucesso.
            return response.data;
        } catch (error) {
            // Loga o erro no console.
            console.log(error);
            
            // Retorna um objeto de erro estruturado.
            response = {
                data: error,
                statusCode: error.status,
                success: false,
                message: error.message,
            }
            return response;
        }
    }

    // Método para realizar requisições PUT.
    async putRequest(endpoint, data, headersParams = {}, api = Global.Api) {
        var response;
        try {
            // Adiciona cabeçalhos à requisição utilizando o método `addHeaders` da classe `Global`.
            await Global.addHeaders(this.client, window.localStorage, headersParams);
            
            // Faz a requisição PUT com os dados fornecidos.
            response = await this.client.put(api + endpoint, data);
            
            // Retorna os dados da resposta em caso de sucesso.
            return response.data;
        } catch (error) {
            // Loga o erro no console.
            console.log(error);
            
            // Retorna um objeto de erro estruturado.
            response = {
                data: error,
                statusCode: error.status,
                success: false,
                message: error.message,
            }
            return response;
        }
    }
}

// Exporta uma instância da classe `GenericService` usando a URL base definida em `Global.Api`.
export default new GenericService(Global.Api);
