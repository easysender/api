﻿using Api.Dtos;
using Api.Models;
using Api.ServiceMOL;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api.Controllers.Api
{
    [RoutePrefix("api/MOL")]
    public class MOLController : ApiController
    {
        private static Entities _context;

        public MOLController()
        {
            _context = new Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        private static Opzioni GetOpzioni(tipoStampa tipoStampa, fronteRetro fronteRetro, ricevutaRitorno ricevutaRitorno)
        {
            var opzioni = new Opzioni();

            var opzioniStampa = new OpzioniStampa();
            opzioniStampa.TipoColore = (tipoStampa == tipoStampa.colori ? TipoColore.COLORE : TipoColore.BW);
            opzioniStampa.FronteRetro = (fronteRetro == fronteRetro.fronte ? true : false);

            var servizio = new OpzioniServizio();
            servizio.Consegna = ModalitaConsegna.S;
            servizio.AttestazioneConsegna = false;

            if (ricevutaRitorno == ricevutaRitorno.si)
                servizio.AttestazioneConsegna = true;

            opzioni.Stampa = opzioniStampa;
            opzioni.Servizio = servizio;

            return opzioni;
        }

        private static RaccomandataMarketServiceClient getNewServiceMOL(Guid guidUser)
        {
            //MULTIPLE USERS
            var u = _context.Users.Where(a => a.guidUser == guidUser);
            if (u.Count() == 0)
                return null;

            var Users = _context.Users.Where(a => a.guidUser == guidUser).SingleOrDefault(a => a.parentId == 0);
            if (Users == null)
                return null;
            RaccomandataMarketServiceClient service = new RaccomandataMarketServiceClient();
            if (Users.areaTestUser)
            {
                service.ClientCredentials.UserName.UserName = Users.usernamePosteAreaTest;
                service.ClientCredentials.UserName.Password = Users.pwdPosteAreaTest;
            }
            else
            {
                service.ClientCredentials.UserName.UserName = Users.usernamePoste;
                service.ClientCredentials.UserName.Password = Users.pwdPoste;
            }
            return service;
        }

        private static Mittente GetMittente(SenderDto sender, string businessName)
        {
            Mittente m = new Mittente();

            m.Nominativo = businessName;
            m.ComplementoIndirizzo = "";
            m.ComplementoNominativo = "";
            m.Indirizzo = sender.dug + " " + sender.address + " " + sender.houseNumber;
            m.Cap = sender.cap;
            m.Comune = sender.city;
            m.Provincia = sender.province;
            m.Nazione = sender.state;

            return m;
        }

        public static Bollettino896 getBollettino896(BulletinsDtos bollettino)
        {
            Bollettino896 b = new Bollettino896();
            b.NumeroContoCorrente = bollettino.numeroContoCorrente;
            b.IntestatoA = bollettino.intestatoA;
            b.FormatoStampa = 0;
            b.AdditionalInfo = "";
            b.IBAN = "";
            b.EseguitoDa = new BollettinoEseguitoDa();
            b.EseguitoDa.Nominativo = bollettino.eseguitoDaNominativo;
            b.EseguitoDa.Indirizzo = bollettino.eseguitoDaIndirizzo;
            b.EseguitoDa.CAP = bollettino.eseguitoDaCAP;
            b.EseguitoDa.Localita = bollettino.eseguitoDaLocalita;
            b.CodiceCliente = bollettino.codiceCliente;
            b.ImportoEuro = bollettino.importoEuro;
            b.Causale = bollettino.causale;
            return b;
        }

        public static Bollettino451 getBollettino451(BulletinsDtos bollettino)
        {
            Bollettino451 b = new Bollettino451();
            b.NumeroContoCorrente = bollettino.numeroContoCorrente;
            b.IntestatoA = bollettino.intestatoA;
            b.FormatoStampa = 0;
            b.AdditionalInfo = "";
            b.IBAN = "";
            b.EseguitoDa = new BollettinoEseguitoDa();
            b.EseguitoDa.Nominativo = bollettino.eseguitoDaNominativo;
            b.EseguitoDa.Indirizzo = bollettino.eseguitoDaIndirizzo;
            b.EseguitoDa.CAP = bollettino.eseguitoDaCAP;
            b.EseguitoDa.Localita = bollettino.eseguitoDaLocalita;
            b.ImportoEuro = bollettino.importoEuro;
            b.Causale = bollettino.causale;
            return b;
        }

        public static Bollettino674 getBollettino674(BulletinsDtos bollettino)
        {
            Bollettino674 b = new Bollettino674();
            b.NumeroContoCorrente = bollettino.numeroContoCorrente;
            b.IntestatoA = bollettino.intestatoA;
            b.FormatoStampa = 0;
            b.AdditionalInfo = "";
            b.IBAN = "";
            b.EseguitoDa = new BollettinoEseguitoDa();
            b.EseguitoDa.Nominativo = bollettino.eseguitoDaNominativo;
            b.EseguitoDa.Indirizzo = bollettino.eseguitoDaIndirizzo;
            b.EseguitoDa.CAP = bollettino.eseguitoDaCAP;
            b.EseguitoDa.Localita = bollettino.eseguitoDaLocalita;
            b.CodiceCliente = bollettino.codiceCliente;
            b.Causale = bollettino.causale;
            return b;
        }

        public static Documento[] getDoc(List<string> strNomeFile, int NumeroDiDocumenti)
        {
            System.IO.FileInfo file;
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            Documento[] ArrayDocumento = new Documento[NumeroDiDocumenti - 1 + 1];
            for (var i = 0; i <= NumeroDiDocumenti - 1; i++)
            {
                Documento documento = new Documento();
                var immagine = System.IO.File.ReadAllBytes(strNomeFile[i]);
                documento.Estensione = "pdf";
                documento.Contenuto = immagine;
                documento.MD5 = System.BitConverter.ToString(md5.ComputeHash(immagine)).Replace("-", string.Empty);
                ArrayDocumento[i] = documento;
            }
            return ArrayDocumento;
        }

        private void createFeatures(tipoStampa tipoStampa, fronteRetro fronteRetro, ricevutaRitorno ricevutaRitorno, int operationId)
        {
            var op1 = new operationFeatures();
            op1.operationId = operationId;
            op1.featureType = "tipo stampa";
            op1.featureValue = Enum.GetName(typeof(tipoStampa), tipoStampa);
            _context.operationFeatures.Add(op1);
            _context.SaveChanges();

            var op2 = new operationFeatures();
            op2.operationId = operationId;
            op2.featureType = "fronte Retro";
            op2.featureValue = Enum.GetName(typeof(fronteRetro), fronteRetro);
            _context.operationFeatures.Add(op2);
            _context.SaveChanges();

            var op3 = new operationFeatures();
            op3.operationId = operationId;
            op3.featureType = "ricevuta Ritorno";
            op3.featureValue = Enum.GetName(typeof(ricevutaRitorno), ricevutaRitorno);
            _context.operationFeatures.Add(op3);
            _context.SaveChanges();

        }

        private static Destinatario GetDestinatarioMOL(NamesDto name, int? index = 1)
        {
            Destinatario n = new Destinatario();
            n.Nominativo = name.businessName + " " + name.surname + " " + name.name;
            n.ComplementoNominativo = name.complementNames;
            n.ComplementoIndirizzo = name.complementAddress;
            n.Indirizzo = name.dug + " " + name.address + " " + name.houseNumber;
            n.Cap = name.cap;
            n.Comune = name.city;
            n.Provincia = name.province;
            n.Nazione = name.state;

            return n;
        }

        private static DestinatarioAR GetDestinatarioARMOL(SenderDto name, int? index = 1)
        {
            DestinatarioAR n = new DestinatarioAR();
            n.Nominativo = name.businessName + " " + name.surname + " " + name.name;
            n.ComplementoNominativo = "";
            n.ComplementoIndirizzo = "";
            n.Indirizzo = name.dug + " " + name.address + " " + name.houseNumber;
            n.Cap = name.cap;
            n.Comune = name.city;
            n.Provincia = name.province;
            n.Nazione = name.state;

            return n;
        }

        [Route("SendNames")]
        [HttpGet]
        public Names sendNames(GetRecipent GetRecipent, int operationId, SenderDto sender, int userId, bool autoconfirm = false)
        {
            var n = _context.Names.SingleOrDefault(a => a.id == GetRecipent.recipient.id);

            var user = _context.Users.SingleOrDefault(a => a.id == userId);
            var guidUser = user.guidUser;

            //PREVERIFICA BOLLETTINO
            if (GetRecipent.bulletin != null)
            {
                ControlloBollettino ctrlB = GlobalClass.verificaBollettino(GetRecipent.bulletin);
                if (!ctrlB.Valido)
                {
                    n.valid = false;
                    n.stato = "Errore nella convalida del bollettino.";
                    _context.SaveChanges();
                    return null;
                }
            }

            RaccomandataMarketServiceClient service = getNewServiceMOL(guidUser);

            var fileName = GetRecipent.recipient.fileName;
            List<string> filesName = new List<string>();
            filesName.Add(fileName);

            tipoStampa ts = tipoStampa.colori;
            if (GetRecipent.recipient.tipoStampa)
                ts = tipoStampa.biancoNero;

            fronteRetro fr = fronteRetro.fronte;
            if (GetRecipent.recipient.fronteRetro)
                fr = fronteRetro.fronteRetro;

            ricevutaRitorno rr = ricevutaRitorno.no;
            if (GetRecipent.recipient.ricevutaRitorno)
                rr = ricevutaRitorno.si;


            Destinatario d = new Destinatario();
            d = GetDestinatarioMOL(GetRecipent.recipient);
            Destinatario[] ld = new Destinatario[1];
            ld[0] = d;
            var request = new InvioRequest();

            var intestazione = new Intestazione();
            intestazione.CodiceContratto =  user.CodiceContrattoMOL;
            intestazione.Prodotto = ProdottoPostaEvo.MOL1;

            var market = new MarketOnline();
            market.AutoConferma = autoconfirm;
            market.Destinatari = ld;
            market.Opzioni = GetOpzioni(ts, fr, rr);
            market.Mittente = GetMittente(sender, user.businessName);
            market.Documenti = getDoc(filesName, 1);

            if (GetRecipent.recipient.ricevutaRitorno) 
                market.DestinatarioAR = GetDestinatarioARMOL(sender);

            request.Intestazione = intestazione;
            request.MarketOnline = market;

            //BOLLETTINO
            if (GetRecipent.bulletin != null)
            {
                PaginaBollettino pagina = new PaginaBollettino();
                object b = null;
                switch (GetRecipent.bulletin.bulletinType)
                {
                    case (int)bulletinType.Bollettino451:
                        b = getBollettino451(GetRecipent.bulletin);
                        pagina.Bollettino = (Bollettino451)b;
                        break;
                    case (int)bulletinType.Bollettino674:
                        b = getBollettino674(GetRecipent.bulletin);
                        pagina.Bollettino = (Bollettino674)b;
                        break;
                    case (int)bulletinType.Bollettino896:
                        b = getBollettino896(GetRecipent.bulletin);
                        pagina.Bollettino = (Bollettino896)b;
                        break;
                }

                PaginaBollettino[] p = new PaginaBollettino[1];
                p[0] = pagina;
                request.MarketOnline.Bollettini = p;
            }

            try
            {
                var esito = service.Invio(request);

                if (esito.Esito == EsitoPostaEvo.OK)
                {
                    Thread.Sleep(5000);

                    var req = new ConfermaInvioRequest();
                    req.CodiceContratto = user.CodiceContrattoMOL;
                    req.IdRichiesta = esito.IdRichiesta;

                    var conferma = service.ConfermaInvio(req);
                    if(conferma.DestinatariRaccomandate == null) {
                        int i = 0;
                        do
                        {
                            Thread.Sleep(5000);
                            conferma = service.ConfermaInvio(req);
                            i++;
                        }
                        while (conferma.DestinatariRaccomandate == null && i < 20);
                    }

                    if (conferma.Esito == EsitoPostaEvo.OK)
                    {
                        n.presaInCaricoDate = conferma.DataAccettazione;
                        n.codice = conferma.DestinatariRaccomandate[0].NumeroRaccomandata;
                        n.stato = "Presa in carico Poste";
                        n.currentState = (int)currentState.PresoInCarico;
                        n.requestId = esito.IdRichiesta;
                        n.valid = true;
                        n.presaInCaricoDate = conferma.DataAccettazione;
                    }
                    if (conferma.Esito == EsitoPostaEvo.KO)
                    {
                        n.stato = "Errore nella conferma di poste. Ritentare l'invio.";
                        n.requestId = null;
                        n.valid = true;
                    }
                    _context.SaveChanges();

                }

                if (esito.Esito == EsitoPostaEvo.KO)
                {
                    n.stato = "Errore nella validazione di poste";
                    n.valid = false;
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                n.valid = false;
                n.stato = "Errore nella richiesta del submit.";
                _context.SaveChanges();
                return n;
            };

            return n;
        }

        [Route("GetState")]
        [HttpGet]
        public ResponseMOLState GetState(string requestId, Guid guidUser)
        {
            var user = _context.Users.Where(a => a.guidUser == guidUser).FirstOrDefault();

            string[] IdRichieste = new string[1];
            IdRichieste[0] = requestId;

            RaccomandataMarketServiceClient service = getNewServiceMOL(guidUser);
            var request = new RecuperaStatoRequest();
            request.CodiceContratto = user.CodiceContrattoMOL;
            request.IdRichieste = IdRichieste;

            var r = new ResponseMOLState();

            var rs = service.RecuperaStato(request);
            if (rs.Esito == EsitoPostaEvo.OK)
            {
                var s = rs.StatoInvii[0];
                r.stato = s.CodiceStatoRichiesta;
                r.descrizioneStato = s.DescrizioneStatoRichiesta;
                r.dataUltimaModifica = s.DataUltimaModifica.ToString();

                var l = GlobalClass.ListOfState();
                var state = l.SingleOrDefault(a => a.identificativo == s.CodiceStatoRichiesta);

                if(state != null) {
                    if (state.tipologia.ToUpper() != "DEFINITIVO") { 
                        var n = _context.Names.SingleOrDefault(a => a.requestId == requestId);
                        if (!state.state)
                            n.valid = false;

                        n.stato = s.DescrizioneStatoRichiesta;
                        _context.SaveChanges();
                    }
                }
            }
            return r;
        }

        [Route("StateRetrive")]
        [HttpGet]
        public ResponseMOLConfirm StateRetrive(string requestId, Guid guidUser)
        {
            var user = _context.Users.Where(a => a.guidUser == guidUser).FirstOrDefault();

            string[] IdRichieste = new string[1];
            IdRichieste[0] = requestId;

            RaccomandataMarketServiceClient service = getNewServiceMOL(guidUser);
            var request = new RecuperaServizioPerIdRichiestaRequest();
            request.CodiceContratto = user.CodiceContrattoMOL;
            request.IdRichieste = IdRichieste;

            var r = new ResponseMOLConfirm();

            var rs = service.RecuperaServizioPerIdRichiesta(request);
            if (rs.Esito == EsitoPostaEvo.OK)
            {
                if (rs.Servizi.Count() > 0)
                {
                    var n = _context.Names.SingleOrDefault(a => a.requestId == requestId);
                    n.presaInCaricoDate = rs.Servizi[0].DataAccettazione;
                    n.codice = rs.Servizi[0].DatiServizio.Destinatari[0].NumeroRaccomandata.Replace(" ", "");
                    n.stato = rs.Servizi[0].StatoServizio;
                    _context.SaveChanges();

                    r.DataAccettazione = (DateTime)rs.Servizi[0].DataAccettazione;
                    r.NumeroRaccomandata = rs.Servizi[0].DatiServizio.Destinatari[0].NumeroRaccomandata.Replace(" ", "");
                    r.EsitoPostaEvo = rs.Esito;
                }
            }
            return r;
        }

        [Route("ResultRetrive")]
        [HttpGet]
        public ResponseMOLConfirm ResultRetrive(string requestId, Guid guidUser)
        {
            var user = _context.Users.Where(a => a.guidUser == guidUser).FirstOrDefault();

            string[] IdRichieste = new string[1];
            IdRichieste[0] = requestId;

            RaccomandataMarketServiceClient service = getNewServiceMOL(guidUser);
            var request = new RecuperaEsitiPerIdRichiestaRequest();
            request.CodiceContratto = user.CodiceContrattoMOL;
            request.IdRichieste = IdRichieste;

            var r = new ResponseMOLConfirm();

            var rs = service.RecuperaEsitiPerIdRichiesta(request);
            if (rs.Esito == EsitoPostaEvo.OK)
            {
                r.EsitoPostaEvo = rs.Esito;
                r.NumeroRaccomandata = rs.RendicontazioneEsiti[0].CodiceTracciatura.Replace(" ", "");
                r.DataAccettazione = rs.RendicontazioneEsiti[0].DataAccettazione;

                var n = _context.Names.SingleOrDefault(a => a.requestId == requestId);
                n.presaInCaricoDate = rs.RendicontazioneEsiti[0].DataAccettazione;
                n.codice = rs.RendicontazioneEsiti[0].CodiceTracciatura.Replace(" ", "");
                _context.SaveChanges();

            }
            return r;
        }

        [Route("StateRetriveForCode")]
        [HttpGet]
        public ResponseMOLConfirm StateRetriveForCode(string code, Guid guidUser)
        {
            var user = _context.Users.Where(a => a.guidUser == guidUser).FirstOrDefault();

            string[] codes = new string[1];
            codes[0] = code;

            RaccomandataMarketServiceClient service = getNewServiceMOL(guidUser);
            var request = new RecuperaServizioPerNumeroRaccomandataRequest();
            request.CodiceContratto = user.CodiceContrattoMOL;
            request.NumeroRaccomandate = codes;


            var r = new ResponseMOLConfirm();

            var rs = service.RecuperaServizioPerNumeroRaccomandata(request);
            if (rs.Esito == EsitoPostaEvo.OK)
            {
                //r.EsitoPostaEvo = rs.Esito;
                //r.NumeroRaccomandata = rs.RendicontazioneEsiti[0].CodiceTracciatura.Replace(" ", "");
                //r.DataAccettazione = rs.RendicontazioneEsiti[0].DataAccettazione;

                //var n = _context.Names.SingleOrDefault(a => a.requestId == requestId);
                //n.presaInCaricoDate = rs.RendicontazioneEsiti[0].DataAccettazione;
                //n.codice = rs.RendicontazioneEsiti[0].CodiceTracciatura.Replace(" ", "");
                //_context.SaveChanges();

            }
            return r;
        }

        [Route("Confirm")]
        [HttpGet]
        public ResponseMOLConfirm Confirm(string requestId, Guid guidUser)
        {
            var user = _context.Users.Where(a => a.guidUser == guidUser).FirstOrDefault();

            var r = new ResponseMOLConfirm();
            RaccomandataMarketServiceClient service = getNewServiceMOL(guidUser);
            var request = new ConfermaInvioRequest();
            request.CodiceContratto = user.CodiceContrattoMOL;
            request.IdRichiesta = requestId;

            var stato = StateRetrive(requestId, guidUser);
            if (stato.EsitoPostaEvo == EsitoPostaEvo.OK)
            {
                r.DataAccettazione = (DateTime)stato.DataAccettazione;
                r.NumeroRaccomandata = stato.NumeroRaccomandata.Replace(" ", "");
                r.EsitoPostaEvo = stato.EsitoPostaEvo;
            }
            else
            {
                var conferma = service.ConfermaInvio(request);
                if(conferma.Esito == EsitoPostaEvo.OK) { 
                    r.DataAccettazione = conferma.DataAccettazione;
                    r.NumeroRaccomandata = conferma.DestinatariRaccomandate[0].NumeroRaccomandata;
                    r.EsitoPostaEvo = conferma.Esito;

                    var n = _context.Names.SingleOrDefault(a => a.requestId == requestId);
                    n.presaInCaricoDate = r.DataAccettazione;
                    n.codice = r.NumeroRaccomandata;
                    n.stato = "Presa in carico Postel";
                    n.currentState = (int)currentState.PresoInCarico;
                    _context.SaveChanges();
                }
            }

            return r;
        }

        [Route("CheckAllFiles")]
        [HttpPost]
        public async Task<GetNumberOfCheckedNames> CheckAllFiles([FromUri]Guid guidUser, [FromBody]ObjectSubmit senderRecipients, [FromUri]bool tsc,
            [FromUri]bool frc, [FromUri]bool rrc, [FromUri]int userId)
        {
            SenderDto sender = new SenderDto();
            sender = senderRecipients.sender;
            List<GetRecipent> GetRecipents = senderRecipients.recipients;

            GetNumberOfCheckedNames ncn = new GetNumberOfCheckedNames();
            List<GetCheckedNames> lgcn = new List<GetCheckedNames>();

            //MULTIPLE USERS
            var users = _context.Users.Where(a => a.guidUser == guidUser);

            //ERRORE GUID
            if (users.Count() == 0)
            {
                ncn.numberOfValidNames = 0;
                ncn.state = "Utente non riconosiuto";
                return ncn;
            }

            //UTENTE INSERITORE
            var u = new Users();
            if (userId > 0)
                u = users.SingleOrDefault(a => a.id == userId);
            else
                u = users.SingleOrDefault(a => a.parentId == 0);


            //ERRORE MITTENTE
            ControlloMittente ctrlM = GlobalClass.verificaMittente(senderRecipients.sender);
            if (!ctrlM.Valido)
            {
                ncn.numberOfValidNames = 0;
                ncn.state = "Mittente non valido";
                return ncn;
            }

            OperationsController oc = new OperationsController();
            OperationsDto op = new OperationsDto();
            op.date = DateTime.Now;
            op.name = " Operazione del " + DateTime.Now.ToString("dd/MM/yyyy");
            op.userId = u.id;
            op.operationType = (int)operationType.MOL;
            op.demoOperation = u.demoUser;
            op.areaTestOperation = u.areaTestUser;
            op.complete = false;
            int operationId = OperationsController.CreateItem(op);

            tipoStampa ts = tipoStampa.colori;
            if (tsc)
                ts = tipoStampa.biancoNero;

            fronteRetro fr = fronteRetro.fronte;
            if (frc)
                fr = fronteRetro.fronteRetro;

            ricevutaRitorno rr = ricevutaRitorno.si;
            if (rrc)
                rr = ricevutaRitorno.no;


            createFeatures(ts, fr, rr, operationId);

            SenderDtos ss = Mapper.Map<SenderDto, SenderDtos>(sender);
            ss.operationId = operationId;
            int senderId = SenderController.CreateItem(ss);

            int validNames = 0;
            foreach (var GetRecipent in GetRecipents.ToList())
            {
                int id = (int)GetRecipent.recipient.id;
                var b = _context.Bulletins.Where(a => a.namesListsId == id).ToList();
                if (b.Count() > 0)
                {
                    GetRecipent.bulletin = Mapper.Map<Bulletins, BulletinsDtos>(b[0]);
                };

                NamesDtos nos = Mapper.Map<NamesDto, NamesDtos>(GetRecipent.recipient);
                nos.operationId = operationId;
                nos.requestId = null;
                nos.guidUser = null;
                nos.valid = true;

                nos.fronteRetro = Convert.ToBoolean(fr);
                nos.ricevutaRitorno = Convert.ToBoolean(rr);
                nos.tipoStampa = Convert.ToBoolean(ts);

                nos.insertDate = DateTime.Now;
                nos.currentState = (int)currentState.inAttesa;

                var nc = new NamesController();
                int idName = nc.CreateItem(nos, u.userPriority);
                if (GetRecipent.bulletin != null)
                {
                    BulletinsDto bos = Mapper.Map<BulletinsDtos, BulletinsDto>(GetRecipent.bulletin);
                    bos.namesId = idName;
                    BulletinsController.CreateItem(bos);
                }
                validNames++;

                GetCheckedNames gcn = new GetCheckedNames()
                {
                    name = nos,
                    valid = true,
                    price = new Prices()
                };

                lgcn.Add(gcn);

            }

            ncn.numberOfValidNames = validNames;
            ncn.checkedNames = lgcn;
            ncn.state = "Inserimento valido!";
            ncn.valid = true;
            ncn.operationId = operationId;

            return ncn;

        }


        [Route("RequestOperationStatus")]
        [HttpGet]
        public List<GetStatoRichiesta> RequestOperationStatus(Guid guidUser, int operationId)
        {
            //MULTIPLE USERS
            var u = _context.Users.Where(a => a.guidUser == guidUser);
            if (u.Count() == 0)
                return null;

            List<GetStatoRichiesta> lGsr = new List<GetStatoRichiesta>();
            var n = _context.Names.Where(a => a.operationId == operationId).Where(a => a.requestId != null).ToList();
            if (n.Count() == 0)
                return null;

            int id = n[0].operationId;
            var o = _context.Operations.SingleOrDefault(a => a.id == id);

            foreach (var name in n)
                {
                    var s = GetState(name.requestId, guidUser);
                    GetStatoRichiesta gsr = new GetStatoRichiesta();
                    gsr.requestId = name.requestId;

                    gsr.statoDescrizione = s.descrizioneStato;
                    gsr.numeroServizio = name.codice;
                    gsr.dataEsito = s.dataUltimaModifica;

                    //name.consegnatoDate = Convert.ToDateTime(gsr.dataEsito);

                    lGsr.Add(gsr);
                }

            return lGsr;
        }

        [Route("RequestNameStatus")]
        [HttpGet]
        public GetStatoRichiesta RequestNameStatus(Guid guidUser, Names name)
        {

            var s = GetState(name.requestId, guidUser);
            GetStatoRichiesta gsr = new GetStatoRichiesta();
            gsr.requestId = name.requestId;

            gsr.statoDescrizione = s.descrizioneStato;
            gsr.numeroServizio = name.codice;
            gsr.dataEsito = s.dataUltimaModifica;

            return gsr;
        }

        [Route("RequestDCS")]
        [HttpGet]
        public string RequestDCS(Guid guidUser, int id)
        {
            var user = _context.Users.Where(a => a.guidUser == guidUser).FirstOrDefault();

            RaccomandataMarketServiceClient service = new RaccomandataMarketServiceClient();
            service = getNewServiceMOL(guidUser);

            var name = _context.Names.SingleOrDefault(a => a.id == id);

            RecuperaDocumentoRequest request = new RecuperaDocumentoRequest();
            request.CodiceContratto = user.CodiceContrattoMOL;
            request.IdRichiesta = name.requestId;

            var dcs = service.RecuperaDocumento(request);
            var nameFile = "";

            if (dcs.Esito == EsitoPostaEvo.OK)
            {
                nameFile = "/public/download/" + DateTime.Now.Ticks + ".pdf";
                var path = HttpContext.Current.Server.MapPath(nameFile);
                System.IO.File.WriteAllBytes(path, dcs.Documento.Contenuto);
            }

            return nameFile;
        }

        private static Documento[] GetDocInsert(byte[] bite)
        {
            System.IO.FileInfo file;
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            Documento[] ArrayDocumento = new Documento[1];
            Documento documento = new Documento();
            file = new System.IO.FileInfo("");
            documento.Estensione = "pdf";
            documento.Contenuto = bite;
            documento.MD5 = System.BitConverter.ToString(md5.ComputeHash(documento.Contenuto)).Replace("-", string.Empty);
            ArrayDocumento[0] = documento;

            return ArrayDocumento;
        }

    }
}
