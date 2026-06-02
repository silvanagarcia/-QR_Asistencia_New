using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;

namespace Asistencia.Infrastructure.Repository
{
    
    public class ControlQRRepository : IControlQRRepository
    {
        static Dictionary<string, string> code = new Dictionary<string, string>();
        public ControlQRRepository()
        {
        }


        public bool GuardaQR(MicroDTO microDTO)
        {
            try
            {
                
                if (!code.ContainsKey(microDTO.Key))
                {
                    code.Add(microDTO.Key, microDTO.Valor);
                }else{
                    code[microDTO.Key]=microDTO.Valor;
                }
                foreach( KeyValuePair<string, string> kvp in code )
                {
                    Console.Write(kvp.Key);
                    Console.WriteLine(kvp.Value);
                }
                return true;
            }
            catch (System.Exception)
            {
                return true;
                throw;
            }
        }

        MicroDTO IControlQRRepository.pedirQR()
        {
            MicroDTO microDTO = new MicroDTO();

            foreach( KeyValuePair<string, string> kvp in code )
            {
                
                microDTO.Key = kvp.Key;
                microDTO.Valor =kvp.Value;
            }

        return microDTO;
        }
    }
}