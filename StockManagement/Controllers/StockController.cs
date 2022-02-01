using Microsoft.AspNetCore.Mvc;
using StockManagement.Command;
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
        IQueryHandler<GetAllStockQuery, StockModel[]> getAllStockQueryHandler;
        IQueryHandler<FindStockByIdQuery, StockModel> findStockByIdQueryHandler;
        ICommandHandler<UpdateStockCommand> updateStockCommandHandler;
        public StockController(IQueryHandler<GetAllStockQuery, StockModel[]> getAllStockQueryHandler,
            IQueryHandler<FindStockByIdQuery, StockModel> findStockByIdQueryHandler,
            ICommandHandler<UpdateStockCommand> updateStockCommandHandler
            )
        {            
            this.getAllStockQueryHandler = getAllStockQueryHandler;
            this.findStockByIdQueryHandler = findStockByIdQueryHandler;
            this.updateStockCommandHandler = updateStockCommandHandler;
        }
        // GET: api/<StockController>
        [HttpGet]
        public IEnumerable<StockModel> GetAll()
        {
            GetAllStockQuery qry = new GetAllStockQuery();
            return getAllStockQueryHandler.Handle(qry);            
        }

        // GET api/<StockController>/5
        [HttpGet("{id}")]
        public StockModel Get(Guid Id)
        {
            FindStockByIdQuery qry = new FindStockByIdQuery() { Id = Id };
            return this.findStockByIdQueryHandler.Handle(qry);
            
        }

        // POST api/<StockController>
        [HttpPost]
        public IActionResult Post([FromBody] StockModel updateStock)
        {
            UpdateStockCommand command = new UpdateStockCommand() { UpdateStock = updateStock };
            updateStockCommandHandler.Handle(command);
            return NoContent();
        }

        // PUT api/<StockController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StockController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
