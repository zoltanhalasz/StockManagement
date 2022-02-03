using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Command
{
    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(TCommand command) where TCommand: ICommand;
    }
    public class CommandDispatcher : ICommandDispatcher
    {
        IServiceProvider service;
        public CommandDispatcher(IServiceProvider service)
        {
            this.service = service;
        }

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            var myType = typeof(ICommandHandler<TCommand>);
            var myService = service.GetService(myType);
            (myService as ICommandHandler<TCommand>).Handle(command);
        }

    }


}
