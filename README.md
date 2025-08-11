# Consulta de Horóscopo em C#

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

## ✒️ Descrição
- Este projeto nasceu de um desafio pessoal que me propus: aprender os conceitos fundamentais de C# e desenvolver uma aplicação funcional em um final de semana. O aplicativo, que roda no terminal, consome uma API de horóscopo que eu já tinha interesse em testar, servindo como uma ótima aplicação prática dos estudos.

## ✨ Funcionalidades

-   **Entrada de Dados do Usuário:** Coleta de nome e data de nascimento com uma validação simples.
-   **Cálculo de Signo:** Determina o signo com base na data de nascimento fornecida.
-   **Consumo de API:** Busca dados de horóscopo diário e mensal de uma API pública.
-   **Tradução Automática:** Utiliza uma segunda API para traduzir os textos do inglês para o português.
-   **Tratamento de Limites:** Como a API de tradução tem um limite de caracteres, a chamada mensal foi feita uma divisão em duas requisições para então juntar novamente.
-   **Interface de Console:** Exibe todas as informações de forma clara e organizada no terminal.

## 🛠️ Tecnologias Utilizadas

-   **Linguagem:** C# (.NET)
-   **Principais Pacotes/Bibliotecas:**
    -   `System.Net.Http`: Chamadas HTTP (`HttpClient`).
    -   `System.Text.Json`: Manipulação de dados JSON.
    -   `System.Globalization`: Validação de datas.
-   **APIs Externas:**
    -   **Horóscopo:** [Horoscope API](https://github.com/sameerkumar18/horoscope-api) (`horoscope-app-api.vercel.app`)
    -   **Tradução:** [MyMemory API](https://mymemory.translated.net/doc/spec.php)

## 🚀 Como Executar

Para executar este projeto localmente, siga os passos abaixo.

**Pré-requisitos:**
-   É necessário ter o [.NET SDK](https://dotnet.microsoft.com/download) instalado em sua máquina.

**Passos:**

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/tiagolucasoo/horoscopo.git
    ```

2.  **Navegue até a pasta do projeto:**
    ```bash
    cd horoscopo
    ```

3.  **Execute o aplicativo:**
    ```bash
    dotnet run
    ```
4.  Siga as instruções no console para inserir seu nome e data de nascimento.

## 🏗️ Estrutura do Código

O projeto está organizado em métodos estáticos dentro da classe `Program`, cada um com uma responsabilidade bem definida:

-   `Main()`: Ponto de entrada do programa.
-   `info_nome()` e `info_nasc()`: Captura e valida os dados de entrada do usuário.
-   `Verificar_Signo()`: Contém a lógica para determinar o signo a partir de uma data.
-   `BuscarHoroscopo()`: O método principal, que realiza as chamadas para as APIs, processa os dados, aplica a lógica de quebra de texto e prepara os resultados para exibição.

## Exemplo de Saída
```bash
 - - - - - - - - - - Consulta de Horoscopo C# - - - - - - - - - -

Digite seu nome: Tiago
Digite sua data de nascimento (Ex: 18/09/2001): 18/09/2001

Olá Tiago! Aguarde, enquanto buscamos o seu horóscopo...

Signo: Virgem
Data: 10/agosto

 - - - - - - - - - - Horóscopo do Dia - 10/agosto - - - - - - - - - -

Você deve desfrutar de um bom humor hoje...
```

## ✍️ Autor
Desenvolvido por **Tiago Lucas Oliveira**.

-   **GitHub:** [tiagolucasoo](https://github.com/tiagolucasoo)
-   **LinkedIn:** [Tiagoollucas](https://www.linkedin.com/in/tiagoollucas/)
