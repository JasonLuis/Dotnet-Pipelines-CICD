O formato YAML (YAML Ain't Markup Language) é utilizado para serializar dados de maneira legível por humanos, muito  utilizado para representar estruturas de dados de forma hierárquica. Devido a sua simplicidade e legibilidade, tem se tornado bem popular entre pessoas desenvolvedoras para configurações, arquivos de manifesto, configurações entre outras tarefas de armazenamento de dados.

Aqui está um exemplo básico de YAML:

```
pipeline:
  - name: build
    trigger: 
      event: push
    steps:
      - name: Checkout
        uses: actions/checkout@v2
      - name: Construir
        run: |
 
  - name: Teste
    trigger:
      event: pull_request
    steps:
      - name: Checkout
        uses: actions/checkout@v2
      - name: Run Tests
        run: |   

  - name: Implantação
    trigger:
      event: push
      branch:
        - main
    steps:
      - name: Checkout
        uses: actions/checkout@v2
      - name: Build
        run: |         
      - name: Deploy to Production
        run: |

```

Algumas características do YAML/YML incluem:

**Legibilidade**: O formato YAML foi desenvolvido para ser compreensível para as pessoas. Ele utiliza uma sintaxe fundamentada em recuos(indentação) para expressar a estrutura, o que facilita o uso de arquivos YAML.

**Facilidade**: O YAML apresenta uma sintaxe simples e transparente, o que o torna a curva de aprendizado bem suave. Não carrega a complexidade de algumas outras linguagens de marcação ou serialização de dados.

**Suporte a vários tipos de dados**:  O suporte a tipos de dados simples, como strings e números, o YAML é capaz de representar tipos de dados mais complexos, tais como listas, dicionários e até mesmo estruturas de dados em aninhamento.

**Adaptabilidade**: Por ser  YAML muito maleável ele pode ser modificado para se adequar a uma miríade de cenários de uso. Ele aceita diretivas específicas e pode ser facilmente incorporado a outras linguagens de programação e frameworks.