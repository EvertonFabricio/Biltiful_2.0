No Banco de dados:
Criei todas as tabelas corretamente (eu acho) e criei as procedures conforme foi surgindo a necessidade delas pelo codigo no programa.

No C#:
Recriei o programa passo a passo, fazendo cada parte  interagir com o banco de dados corretamente. O Codigo antigo estava muito confuso e mais da metade das funções não fazia sentido, pq era tudo pra salvar em arquivo e nao no banco. 

O que está funcionando:
Do Cliente:

*Cadastrar cliente, com validação de cpf;
*Listar todos os clientes, com proximo e anterior bonitinho;
*Editar registro do cliente, editando apenas o campo informado e não mexendo nos outros;
*Bloquear e desbloquear o cliente (na verdade ele só abre a alteração e pede pra informar se está ativo ou não. Não fiz tratamento pra saber qual a situação atual antes de alterar.
*Localizar cliente, tanto pelo CPF como pelo nome, com busca dinamica (qualquer parte do nome ou cpf)
*Localizar cliente bloqueado. Ele faz a pesquisa e retorna o nome do cliente e a situação (bloqueado ou desbloqueado)

Do Fornecedor:
*As mesmas coisas do cliente! 

Da Materia Prima:
*Cadastro de materia prima, com ID certinho buscando no banco qual o ultimo ID e criando o proximo.
*Localizar Materia Prima, com busca dinamica pelo nome da MP ou pelo ID. Retornando todas as informações da MP.



O que falta fazer:

Listar as Materias Prima;
Alterar a situação da Materia Prima;
TUDO do cadastro de Produto;
TUDO da Produção;
TUDO da Compras;
TUDO do Vendas.

