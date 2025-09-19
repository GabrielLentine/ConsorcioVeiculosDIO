namespace ConsorcioVeiculos.Dominio.ModelViews;
public class Home
{
    public string Mensagem { get =>
            "Bem-vindo a esta Minimal API construída juntamente com o Bootcamp 'Avanade - Back-end com .NET e IA'," +
            " realizado pela DIO, em 2025"; }
    public string? Documentacao { get =>
            "para ver a documentação e os endpoints, coloque /swagger depois do 'localhost:7129'"; }

    public string Github { get =>
            "O repositório deste projeto está disponível em meu GitHub de estudos: https://github.com/GabrielLentine"; }
}
