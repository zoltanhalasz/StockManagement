using Microsoft.AspNetCore.Mvc;
using StockManagement.Command;
using StockManagement.Events;
using StockManagement.Models;
using StockManagement.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {        
        IQueryDispatcher queryDispatcher;
        ICommandDispatcher commandDispatcher;
        ITableStorageRepository tableStorageRepository;
        public SupplierController(
            IQueryDispatcher queryDispatcher,
            ICommandDispatcher commandDispatcher,
            ITableStorageRepository tableStorageRepository
            )
        {            

            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
            this.tableStorageRepository = tableStorageRepository;
        }
        // GET: api/<StockController>
        [HttpGet]
        public GetAllSuppliersQueryResult GetAll()
        {
            GetAllSuppliersQuery qry = new GetAllSuppliersQuery();
            return queryDispatcher.Dispatch<GetAllSuppliersQuery, GetAllSuppliersQueryResult>(qry);            
        }

        // GET api/<StockController>/5
        [HttpGet("{id:Guid}")]
        public FindSupplierByIdQueryResult Get(Guid id)
        {
            FindSupplierByIdQuery qry = new FindSupplierByIdQuery() { Id = id };
            return queryDispatcher.Dispatch<FindSupplierByIdQuery, FindSupplierByIdQueryResult>(qry);
        }

        // GET api/<StockController>/5
        [HttpGet("history/{id:Guid}")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            return Ok(await tableStorageRepository.GetEventsById(id));
        }


        // POST api/<StockController>
        [HttpPost]
        public IActionResult Post([FromBody] SupplierModel createSupplier)
        {
            CreateSupplierCommand command = new CreateSupplierCommand() { CreateSupplier= createSupplier};
            commandDispatcher.Dispatch(command);
            return NoContent();
        }

        [HttpPost("addstocktosupplier")]
        public IActionResult AddStockToSupplier([FromBody] SupplierStock supplStock)
        {
            AddStockToSupplierCommand command = new AddStockToSupplierCommand() { SupplierId = supplStock.SupplierId, StockId = supplStock.StockId };
            commandDispatcher.Dispatch(command);
            return NoContent();
        }

        [HttpPost("removestockfromsupplier")]
        public IActionResult RemoveStockFromSupplier([FromBody] SupplierStock supplStock)
        {
            RemoveStockFromSupplierCommand command = new RemoveStockFromSupplierCommand() { SupplierId = supplStock.SupplierId, StockId = supplStock.StockId };
            commandDispatcher.Dispatch(command);
            return NoContent();
        }

        // PUT api/<StockController>/5
        [HttpPut]
        public IActionResult Put([FromBody] SupplierModel updateSupplier)
        {
            UpdateSupplierCommand command = new UpdateSupplierCommand() { UpdateSupplier = updateSupplier };
            commandDispatcher.Dispatch(command);
            return NoContent();
        }

        // DELETE api/<StockController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            DeleteSupplierCommand command = new DeleteSupplierCommand() { SupplierId= id};
            commandDispatcher.Dispatch(command);
            return NoContent();
        }
    }
}
