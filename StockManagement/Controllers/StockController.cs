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
    public class StockController : ControllerBase
    {        
        IQueryDispatcher queryDispatcher;
        ICommandDispatcher commandDispatcher;
        ITableStorageRepository tableStorageRepository;
        public StockController(
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
        public GetAllStockQueryResult GetAll()
        {
            GetAllStockQuery qry = new GetAllStockQuery();
            return queryDispatcher.Dispatch<GetAllStockQuery, GetAllStockQueryResult>(qry);            
        }

        // GET api/<StockController>/5
        [HttpGet("{id:Guid}")]
        public FindStockByIdQueryResult Get(Guid id)
        {
            FindStockByIdQuery qry = new FindStockByIdQuery() { Id = id };
            return queryDispatcher.Dispatch<FindStockByIdQuery, FindStockByIdQueryResult>(qry);
        }

        // GET api/<StockController>/5
        [HttpGet("history/{id:Guid}")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            return Ok(await tableStorageRepository.GetEventsByStockId(id));
        }


        // POST api/<StockController>
        [HttpPost]
        public IActionResult Post([FromBody] StockModel createStock)
        {
            CreateStockCommand command = new CreateStockCommand() { CreateStock= createStock };
            commandDispatcher.Dispatch(command);
            return NoContent();
        }

        // PUT api/<StockController>/5
        [HttpPut]
        public IActionResult Put([FromBody] StockModel updateStock)
        {
            UpdateStockCommand command = new UpdateStockCommand() { UpdateStock = updateStock };
            commandDispatcher.Dispatch(command);
            return NoContent();
        }

        // DELETE api/<StockController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            DeleteStockCommand command = new DeleteStockCommand() { Id= id};
            commandDispatcher.Dispatch(command);
            return NoContent();
        }
    }
}
