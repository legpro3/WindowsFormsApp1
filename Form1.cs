using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Npgsql;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            bool enderecoEncontrado = true;
            while (enderecoEncontrado)
            {
                string cep = txtCEP.Text;
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        var endereco = JsonConvert.DeserializeObject<Endereco>(content);
                        if (endereco != null && !string.IsNullOrEmpty(endereco.CEP))
                        {
                            MessageBox.Show("Endereço encontrado." +
                                $"\nCEP: {cep}" +
                                $"\nLogradouro: {endereco.Logradouro}" +
                                $"\nComplemento: {endereco.Complemento}" +
                                $"\nBairro: {endereco.Bairro}" +
                                $"\nLocalidade: {endereco.Localidade}" +
                                $"\nUF: {endereco.UF}" +
                                $"\nIBGE: {endereco.IBGE}" +
                                $"\nGIA: {endereco.GIA}" +
                                $"\nDDD: {endereco.DDD}" +
                                $"\nSIAFI: {endereco.SIAFI}");

                            bool registroExistente = VerificarRegistroExistenteNoBancoDeDados(endereco.CEP);
                            if (registroExistente)
                            {
                                AtualizarEnderecoNoBancoDeDados(endereco);
                                MessageBox.Show("Endereço atualizado no banco de dados.");
                            }
                            else
                            {
                                InserirEnderecoNoBancoDeDados(endereco);
                                MessageBox.Show("Endereço inserido no banco de dados.");
                            }
                            DialogResult result = MessageBox.Show("Deseja excluir o endereço?", "Exclusão de Endereço", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                RemoverEnderecoDoBancoDeDados(endereco.CEP);
                                MessageBox.Show("Endereço excluído do banco de dados.");
                            }
                            enderecoEncontrado = false;
                        }
                        else
                        {
                            MessageBox.Show("Endereço não encontrado. Por favor, insira o CEP novamente.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erro. Por favor, insira o CEP novamente.");
                    }
                }
            }
        }

        private void InserirEnderecoNoBancoDeDados(Endereco endereco)
        {
            {
                using (var connection = new NpgsqlConnection("Host=localhost:5432;Username=postgres;Password=1478951;Database=postgres"))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("INSERT INTO endereco (cep, logradouro, complemento, bairro, localidade, uf, ibge, gia, ddd, siafi) " +
                                                           "VALUES (@cep, @logradouro, @complemento, @bairro, @localidade, @uf, @ibge, @gia, @ddd, @siafi)", connection))
                    {
                        command.Parameters.AddWithValue("@cep", endereco?.CEP);
                        command.Parameters.AddWithValue("@logradouro", endereco?.Logradouro);
                        command.Parameters.AddWithValue("@complemento", endereco?.Complemento);
                        command.Parameters.AddWithValue("@bairro", endereco?.Bairro);
                        command.Parameters.AddWithValue("@localidade", endereco?.Localidade);
                        command.Parameters.AddWithValue("@uf", endereco?.UF);
                        command.Parameters.AddWithValue("@ibge", endereco?.IBGE);
                        command.Parameters.AddWithValue("@gia", endereco?.GIA);
                        command.Parameters.AddWithValue("@ddd", endereco?.DDD);
                        command.Parameters.AddWithValue("@siafi", endereco?.SIAFI);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void AtualizarEnderecoNoBancoDeDados(Endereco endereco)
        {
            using (var connection = new NpgsqlConnection("Host=localhost:5432;Username=postgres;Password=1478951;Database=postgres"))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("UPDATE endereco SET logradouro=@logradouro, complemento=@complemento, " +
                                                       "bairro=@bairro, localidade=@localidade, uf=@uf, ibge=@ibge, gia=@gia, ddd=@ddd, siafi=@siafi " +
                                                       "WHERE cep=@cep", connection))
                {
                    command.Parameters.AddWithValue("@logradouro", endereco?.Logradouro);
                    command.Parameters.AddWithValue("@complemento", endereco?.Complemento);
                    command.Parameters.AddWithValue("@bairro", endereco?.Bairro);
                    command.Parameters.AddWithValue("@localidade", endereco?.Localidade);
                    command.Parameters.AddWithValue("@uf", endereco?.UF);
                    command.Parameters.AddWithValue("@ibge", endereco?.IBGE);
                    command.Parameters.AddWithValue("@gia", endereco?.GIA);
                    command.Parameters.AddWithValue("@ddd", endereco?.DDD);
                    command.Parameters.AddWithValue("@siafi", endereco?.SIAFI);
                    command.Parameters.AddWithValue("@cep", endereco?.CEP);

                    command.ExecuteNonQuery();
                }
            }
        }


        private bool VerificarRegistroExistenteNoBancoDeDados(string cep)
        {
            using (var connection = new NpgsqlConnection("Host=localhost:5432;Username=postgres;Password=1478951;Database=postgres"))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT COUNT(*) FROM endereco WHERE cep=@cep", connection))
                {
                    command.Parameters.AddWithValue("@cep", cep);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        private void RemoverEnderecoDoBancoDeDados(string cep)
        {
            {
                using (var connection = new NpgsqlConnection("Host=localhost:5432;Username=postgres;Password=1478951;Database=postgres"))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("DELETE FROM endereco WHERE cep=@cep", connection))
                    {
                        command.Parameters.AddWithValue("@cep", cep);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
   public class Endereco
    {
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string UF { get; set; }
        public string IBGE { get; set; }
        public string GIA { get; set; }
        public string DDD { get; set; }
        public string SIAFI { get; set; }
    }