# WTV_Trivia
  - Criação de um serviço web que permita jogar um jogo de trivia online. O jogo consiste em responder a 5 perguntas de trivia e, no fim, será atribuída uma pontuação.  

  - O serviço deve ter um sistema de login com username e password. Deve-se guardar em base de dados o username e password assim como a pontuação.  
  - O Front-end deve ser feito em VueJS e o backend deve ser feito em ASP.NET Core (.Net 5) A base de dados pode ser usado SQL(mysql, sql server, postgres) ou NOSQL(mongodb) 
  - O design assim como a informação a mostrar e como mostrar fica ao critério de quem tiver a programar Como fonte de perguntas deve usar-se o serviço externo     https://opentdb.com.  
  - O código deve ser colocado num repositório do github com um readme a explicar como correr o serviço  Bonus: imagem docker do serviço pronta a ser usada Bonus 2: script docker-compose para arranques dos serviços

  - Esta a ser usada a interface ASP.NET Core Identity para Gestão de Roles e Utilizadores

# Setup
 - # MongoDB Database:
  - A base de dados está em MongoDB hospedada no MongoDB Atlas - não serão necessários passos para configuração da base de dados (Caso seja necessário acederem à base de dados posso dar acesso ao meu cluster)
  - Se necessário configurarem uma base de dados local basta irem ao ficheiro do projecto "WTV_TriviaGame/appsettings.json" e alterarem o valor indicado na key, "MongoConnnectionString" para a ConnectionString do server utilizado o Nome da Base de dados deve ser "TriviaDB"

 - # MongoDB DataStructure
    - Nome da DB: TriviaDB
      - TriviaDB.Users
        - _id:60ee0b0efa1be564288c2e93
        - UserName:"Admin@mail.com"
        - NormalizedUserName:"ADMIN@MAIL.COM"
        - Email:"Admin@mail.com"
        - NormalizedEmail:null
        - EmailConfirmed:false
        - PasswordHash:"AQAAAAEAACcQAAAAEIv4d09dqn0VZJRUgUeMiMS2/bSyNeNnSnqAJB2NbuWmNZozmdWghM..."
        - SecurityStamp:null
        - ConcurrencyStamp:"a655adc7-c4f4-4815-b059-1fdbef545e90"
        - PhoneNumber:null
        - PhoneNumberConfirmed:false
        - TwoFactorEnabled:false
        - LockoutEnd:null
        - LockoutEnabled:false
        - AccessFailedCount:0
        - Name:"Admin"
        - IsAdmin:true
        - Roles:
          -Array
            - 0:"ADMIN"1:"USER"
      - TriviaDB.Roles:
        - _id:60ee0a6efa1be564288c2e92 
        - Name:"User"
        - NormalizedName:"USER"
        - ConcurrencyStamp:"c3752b3a-1258-4e35-a6a4-a62c0c62ee2f"
      - TriviaDB.Trivia:
        - _id:60ee0aca8f47519eb63cf2b0
        - UserId:60ee0a6dfa1be564288c2e91
        - Perguntas:null
        - Points:"0"

 - # App Running:
  - Para correr a aplicação basta executar o DockerFile ou executar localmente
  


