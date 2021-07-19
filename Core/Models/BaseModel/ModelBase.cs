using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Base
{
    public abstract class ModelBase
    {
        #region [ Contructor ]

        /// <summary>
        /// No construtor. As propriedades <see cref="CreationDate"/> e <see cref="UpdateDate"/> são inicializadas,
        /// a propriedade <see cref="CreationDate"/> recebe como valor o horário atual do sistema através do <see cref="DateTime.Now"/>
        /// e a propriedade <see cref="UpdateDate"/> recebe o valor da <see cref="CreationDate"/>
        /// </summary>
        public ModelBase()
        {
            CreationDate = DateTime.UtcNow;
            UpdateDate = CreationDate;
        }

        #endregion

        /// <summary>
        /// Seta ou retorna a data e hora da criação do registro no banco de dados.
        /// </summary>
        [Required]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Seta ou retorna a data e hora da última alteração do registro no banco de dados.
        /// </summary>
        [Required]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Seta ou retorna se o registro foi "removido" do banco de dados.
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; }
    }
}
