// --------------------------------------------------------------------------------------------------------------------
// <copyright file=" ProductRepositoryTest.cs" company="EPIC Software">
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
// </copyright>
// <summary>
//  Contributors: Roy Gonzalez
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Mobilize.Sample.Infraestructure.Test
{
    using System.Linq;

    using Epic.Sample.Domain.Entities;
    using Epic.Sample.Domain.Repository;
    using Epic.Sample.Infrastructure;

    using Xunit;

    public class ProductRepositoryTest
    {
        [Fact]
        public void TheProductShouldBePersisted()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                var product = new Product { Name = "product", Price = 100.0, Quantity = 10.0 };
                var repository = uow.Repository<IProductRepository>();
                repository.Save(product);
                uow.Commit();
            }

            using (IUnitOfWork uow = new UnitOfWork())
            {
                var repository = uow.Repository<IProductRepository>();
                var product = repository.Find(c => c.Name == "product");
                Assert.True(product.Price == 100.0);
                Assert.True(product.Quantity == 10.0);
            }

            using (IUnitOfWork uow = new UnitOfWork())
            {
                var repository = uow.Repository<IProductRepository>();
                var product = repository.FindAll(c => c.Name == "product");
                Assert.True(product.Any());
            }

            using (IUnitOfWork uow = new UnitOfWork())
            {
                var repository = uow.Repository<IProductRepository>();
                var product = repository.Find(c => c.Name == "product");
                product.Name = "other Name";
                repository.Update(product);
            }
        }
    }
}