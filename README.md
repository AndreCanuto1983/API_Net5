WebAPICore3.1

Essenciais para visualizar o projeto
 - Visual studio 2019 : https://visualstudio.microsoft.com/pt-br/vs/
 - Sql Server : https://www.microsoft.com/en-US/download/details.aspx?id=53167

Modo de uso
1 - Após instalar os programas acima, abra o projeto
2 - Back.sln
3 - Verifique se o projeto WebAPI está definido como projeto de inicialização
		- Caso não, clique com o botão direito do mouse sobre ele e clique sobre "Definir como Projeto de Inicialização"
		- Verifique se o VS instalou as dependências do projeto
4 - Dentro do projeto WebAPI, localize o arquivo: appsettings.json
5 - Dentro do arquivo "appsettings.json" localize: "DefaultConnection" e insira a sua string de conexão do seu banco de dados sql  
6 - Abra o menu: Ferramentas > Gerenciador de Pacotes do NuGet > Console do Gerenciador de Pacotes
7 - Digite: Update-Database
		- O VS vai criar o banco de dados para você
8 - Rode o proejeto, abrirá uma página do navegador com a documentação da API pelo SWAGGER
		- Se preferir, deixei um Json do postman já configurado os requests para teste é só importar no Postman
			- arquivo: ApiSwagger_AndreCanuto.postman_collection.json
9 - Quando registrar um usuário, a API vai retornar um token para acesso aos demais end points, nem precisará utilizar o end point de login se não quiser.
		- Para configurar o token pelo SWAGGER, clique em Authorize e no campo digitável insira:  Bearer { adicione o teken gerado, lembre-se de tirar as chaves aqui }
10 - Registre uma pessoa, informando sua cidade e dados necessários solicitados pela ApiSwagger_AndreCanuto
11 - Através do get do end point do person é que vai exibir a playlist de acordo com a temperatura atual da sua cidade, além de retornar os dados da pessoa.