Nesta atividade de preparação do ambiente vamos adicionar a biblioteca Bogus que nos permite gerar uma massa de dados, que será utilizada na implementação dos testes desta aula, portanto siga os passos a seguir para configurar seu projeto e tirar o maior proveito dos vídeos.

Vamos começar realizando a instalação da biblioteca Bogus, para isso acesso o gerenciador de pacotes do nuget, você pode fazer clicando com o botão direito sobre a dependências do projeto de teste e clicar em **Gerenciador de pacotes do Nuget**, em seguida pesquisar por Bogus como mostra a imagem abaixo:

![Print de parte da tela do gerenciador de pacotes do nuget, mostrando o campo de procura com a palavra Bogus e na mesma imagem mais abaixo a descrição da biblioteca Bogus destacada em um quadro vermelho.](https://cdn1.gnarususercontent.com.br/1/558587/42e2d7a5-d9b0-4e5c-9c85-44e7b74fc0bf.png)  


Em seguida, você deverá clicar em instalar e aceitar os termos de utilização da biblioteca.Com a instalação concluída, o próximo passo de configuração é definir no projeto de testes uma pasta, que estamos nomeando como **DataBuilders** e neste diretório iremos criar as classes que irão gerar a massa de dados utilizando a biblioteca Bogus que serão usadas nos testes desta aula. Abaixo a codificação das classes:

```
//OfertaViagemDataBuilder 
internal class OfertaViagemDataBuilder : Faker<OfertaViagem>
{
    public Rota? Rota { get; set; }
    public Periodo? Periodo { get; set; }
    public double PrecoMinimo { get; set; } = 1;
    public double PrecoMaximo { get; set; } = 100.0;

    public OfertaViagemDataBuilder()
    {
        CustomInstantiator(f => {
            Periodo periodo = Periodo ?? new PeriodoDataBuilder().Build();
            Rota rota = Rota ?? new RotaDataBuilder().Build();
            double preco = f.Random.Double(PrecoMinimo, PrecoMaximo);
            return new OfertaViagem(rota, periodo, preco);
        });
    }
}

```

e

```
//PeriodoDataBuilder 
internal class PeriodoDataBuilder : Faker<Periodo>
{
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public PeriodoDataBuilder()
    {
        CustomInstantiator(f =>
        {
            DateTime dataInicio = DataInicial ?? f.Date.Soon();
            DateTime dataFinal = DataFinal ?? dataInicio.AddDays(30);
            return new Periodo(dataInicio, dataFinal);
        });
    }

    public Periodo Build() => Generate();
}
```

E por fim:

```
internal class RotaDataBuilder : Faker<Rota>
{
    public string? Origem { get; set; }
    public string? Destino { get; set; }

    public RotaDataBuilder()
    {
        CustomInstantiator(f =>
        {
            string origem = Origem ?? f.Lorem.Sentence(2);
            string destino = Destino ?? f.Lorem.Sentence(2);
            return new Rota(origem, destino);
        });
    }

    public Rota Build() => Generate();
}
```

O código final referente a esta atividade de **Preparação do ambiente** está disponível no [repositório do GitHub](https://github.com/alura-cursos/JornadaMilhas-API-rec/tree/aula04-PreparandoAmbiente) para que você possa consultar e tirar dúvidas.