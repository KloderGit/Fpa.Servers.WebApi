using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiTests
{
    public class Services
    {
        public IDataManager GetCRM()
        {
            Connection connection = new Connection("apfitness", "robot@fitness-pro.ru", "249bb59ec06e124d685da4b183e01644ab601f66");
            return new CrmManager(connection);
        }

        public UnitOfWork Get1C()
        {
            return new UnitOfWork("Kloder", "Kaligula2");
        }

        public TypeAdapterConfig GetMapper()
        {
            return  new TypeAdapterConfig();

        }





    }
}
