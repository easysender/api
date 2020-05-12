using Api.Dtos;
using Api.Models;
using Api.ServiceLol;
using Api.ServiceRol;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers.Api
{
    [RoutePrefix("api/Sending")]
    public class SynchronizeSendingController : ApiController
    {

        private Entities _context;

        public SynchronizeSendingController()
        {
            _context = new Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Route("Sync")]
        [HttpGet]
        public async Task<bool> Sync()
        {
            var namesGrouped = _context.Names
                .Where(a => a.valid == true)
                .Where(a => a.currentState == 0)
                .Where(a => a.requestId == null)
                .Where(a => a.Operations.demoOperation == false)
                .Where(a => a.Operations.complete == true)
                .Where(a => a.locked == false)
                .Take(30)
                .GroupBy(a => a.operationId)
                .ToList();

            var guid = Guid.NewGuid();

            LockUnlockNames(namesGrouped, true, guid);

            try
            {
                for (var i = 0; i < namesGrouped.Count; i++)
                {
                    var list = new List<Names>();
                    var names = namesGrouped[i].ToList();
                    foreach (var n in names)
                    {
                        var sender = _context.Senders.SingleOrDefault(a => a.operationId == n.Operations.id);
                        var bulletin = _context.Bulletins.Where(a => a.namesId == n.id).ToList();
                        Bulletins b = null;
                        if (bulletin.Count() > 0)
                        {
                            b = bulletin[0];
                        }

                        var senderDto = Mapper.Map<Senders, SenderDto>(sender);

                        var gr = new GetRecipent()
                        {
                            recipient = Mapper.Map<Names, NamesDto>(n),
                            bulletin = Mapper.Map<Bulletins, BulletinsDtos>(b)
                        };

                        switch (n.Operations.operationType)
                        {
                            case (int)operationType.MOL:
                                var m = new MOLController();
                                var newNamesMol = m.sendNames(gr, n.operationId, senderDto, n.Operations.userId);
                                if (newNamesMol.valid)
                                    list.Add(newNamesMol);
                                break;
                            case (int)operationType.COL:
                                var c = new COLController();
                                var newNamesCol = c.sendNames(gr, n.operationId, senderDto, n.Operations.userId);
                                if (newNamesCol.valid)
                                    list.Add(newNamesCol);
                                break;
                            case (int)operationType.ROL:
                                var r = new RolController();
                                var newNamesRol = await r.sendNames(gr, n.operationId, senderDto, n.Operations.userId);
                                if (newNamesRol.valid)
                                    list.Add(newNamesRol);
                                break;
                        }
                    }
                    if (list.Count > 0)
                        switch (names[0].Operations.operationType)
                        {
                            case (int)operationType.MOL:
                                var m = new MOLController();
                                break;

                            case (int)operationType.COL:
                                var c = new COLController();
                                break;

                            case (int)operationType.ROL:
                                var r = new RolController();
                                await r.ValorizzaConfirm(list, names[0].Operations.Users.guidUser);
                                break;
                        }

                }
            }
            catch (Exception e)
            {

            }

            LockUnlockNames(namesGrouped, false, guid);

            return true;
        }


        private void LockUnlockNames(List<IGrouping<int, Names>>namesGrouped, bool toLock, Guid guid)
        {
            if(toLock)
                for (var i = 0; i < namesGrouped.Count; i++)
                {
                    namesGrouped[i].ToList().ForEach(x => {
                        x.locked = true;
                        x.reSendGuid = guid; 
                    });
                    _context.SaveChanges();
                }
            else
                for (var i = 0; i < namesGrouped.Count; i++)
                {
                    namesGrouped[i].ToList().ForEach(x => {
                        x.locked = false;
                        x.reSendGuid = null;
                    });
                    _context.SaveChanges();
                }
        }

    }
}
