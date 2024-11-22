Esta atividade de preparação do ambiente vai ser requerida quando estamos criando uma conta de utilização do Azure Devops e usamos nossa primeira pipeline de execução. Durante a execução poderá acontecer um erro **`##[error]No hosted parallelism has been purchased or granted.`**


![alt text: A imagem mostra o recorte da página de agendamento e execução da pipeline destacando o erro “No hosted parallelism has been purchased or granted”.](http://cdn3.gnarususercontent.com.br/3661-testes-dot-net-4/imagem13.png)

Este erro ocorre em virtude da Microsoft implementar uma série de verificações de segurança referente à organização e projeto criado, para utilização do recurso de **`paralelismo`**.

O paralelismo em pipelines de CI/CD permite que várias tarefas sejam executadas simultaneamente, o que pode acelerar significativamente o tempo de construção e implantação de projetos. No entanto, muitas plataformas de CI/CD têm limites de paralelismo, dependendo do plano ou da assinatura do usuário, lembre-se que estamos trabalhando com a versão gratuita do serviço.

Para a resolução deste problema e a liberação da sua pipeline você deverá acessar este [link](https://aka.ms/azpipelines-parallelism-request) e preencher um formulário, que é mostrado abaixo:

![alt text: A imagem mostra o recorte da página do formulário de liberação de utilização do recurso de paralelismo no Azure Devops.](http://cdn3.gnarususercontent.com.br/3661-testes-dot-net-4/imagem14.png)

Após o preenchimento do formulário com informações de nome, email, organização e sobre a visibilidade do projeto você receberá um email da Microsoft liberando seu projeto para executar a pipeline, a resposta pode variar  entre 24h a 48h.Com esta etapa concluída, a execução da pipeline criada irá funcionar. Bons estudos!