using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Servicing.Data;

namespace Servicing
{
    public partial class sql_rendimento_consignadoService
    {
        sql_rendimento_consignadoContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly sql_rendimento_consignadoContext context;
        private readonly NavigationManager navigationManager;

        public sql_rendimento_consignadoService(sql_rendimento_consignadoContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportCedentesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/cedentes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/cedentes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCedentesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/cedentes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/cedentes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCedentesRead(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Cedentes> items);

        public async Task<IQueryable<Servicing.Models.sql_rendimento_consignado.Cedentes>> GetCedentes(Query query = null)
        {
            var items = Context.Cedentes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCedentesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCedentesGet(Servicing.Models.sql_rendimento_consignado.Cedentes item);
        partial void OnGetCedentesByIdCedente(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Cedentes> items);


        public async Task<Servicing.Models.sql_rendimento_consignado.Cedentes> GetCedentesByIdCedente(long idcedente)
        {
            var items = Context.Cedentes
                              .AsNoTracking()
                              .Where(i => i.IdCedente == idcedente);

 
            OnGetCedentesByIdCedente(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCedentesGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCedentesCreated(Servicing.Models.sql_rendimento_consignado.Cedentes item);
        partial void OnAfterCedentesCreated(Servicing.Models.sql_rendimento_consignado.Cedentes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Cedentes> CreateCedentes(Servicing.Models.sql_rendimento_consignado.Cedentes cedentes)
        {
            OnCedentesCreated(cedentes);

            var existingItem = Context.Cedentes
                              .Where(i => i.IdCedente == cedentes.IdCedente)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Cedentes.Add(cedentes);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(cedentes).State = EntityState.Detached;
                throw;
            }

            OnAfterCedentesCreated(cedentes);

            return cedentes;
        }

        public async Task<Servicing.Models.sql_rendimento_consignado.Cedentes> CancelCedentesChanges(Servicing.Models.sql_rendimento_consignado.Cedentes item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCedentesUpdated(Servicing.Models.sql_rendimento_consignado.Cedentes item);
        partial void OnAfterCedentesUpdated(Servicing.Models.sql_rendimento_consignado.Cedentes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Cedentes> UpdateCedentes(long idcedente, Servicing.Models.sql_rendimento_consignado.Cedentes cedentes)
        {
            OnCedentesUpdated(cedentes);

            var itemToUpdate = Context.Cedentes
                              .Where(i => i.IdCedente == cedentes.IdCedente)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(cedentes);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCedentesUpdated(cedentes);

            return cedentes;
        }

        partial void OnCedentesDeleted(Servicing.Models.sql_rendimento_consignado.Cedentes item);
        partial void OnAfterCedentesDeleted(Servicing.Models.sql_rendimento_consignado.Cedentes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Cedentes> DeleteCedentes(long idcedente)
        {
            var itemToDelete = Context.Cedentes
                              .Where(i => i.IdCedente == idcedente)
                              .Include(i => i.Lotes)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCedentesDeleted(itemToDelete);


            Context.Cedentes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCedentesDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportClientesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/clientes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/clientes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportClientesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/clientes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/clientes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnClientesRead(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Clientes> items);

        public async Task<IQueryable<Servicing.Models.sql_rendimento_consignado.Clientes>> GetClientes(Query query = null)
        {
            var items = Context.Clientes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnClientesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnClientesGet(Servicing.Models.sql_rendimento_consignado.Clientes item);
        partial void OnGetClientesByIdCliente(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Clientes> items);


        public async Task<Servicing.Models.sql_rendimento_consignado.Clientes> GetClientesByIdCliente(long idcliente)
        {
            var items = Context.Clientes
                              .AsNoTracking()
                              .Where(i => i.IdCliente == idcliente);

 
            OnGetClientesByIdCliente(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnClientesGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnClientesCreated(Servicing.Models.sql_rendimento_consignado.Clientes item);
        partial void OnAfterClientesCreated(Servicing.Models.sql_rendimento_consignado.Clientes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Clientes> CreateClientes(Servicing.Models.sql_rendimento_consignado.Clientes clientes)
        {
            OnClientesCreated(clientes);

            var existingItem = Context.Clientes
                              .Where(i => i.IdCliente == clientes.IdCliente)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Clientes.Add(clientes);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(clientes).State = EntityState.Detached;
                throw;
            }

            OnAfterClientesCreated(clientes);

            return clientes;
        }

        public async Task<Servicing.Models.sql_rendimento_consignado.Clientes> CancelClientesChanges(Servicing.Models.sql_rendimento_consignado.Clientes item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnClientesUpdated(Servicing.Models.sql_rendimento_consignado.Clientes item);
        partial void OnAfterClientesUpdated(Servicing.Models.sql_rendimento_consignado.Clientes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Clientes> UpdateClientes(long idcliente, Servicing.Models.sql_rendimento_consignado.Clientes clientes)
        {
            OnClientesUpdated(clientes);

            var itemToUpdate = Context.Clientes
                              .Where(i => i.IdCliente == clientes.IdCliente)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(clientes);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterClientesUpdated(clientes);

            return clientes;
        }

        partial void OnClientesDeleted(Servicing.Models.sql_rendimento_consignado.Clientes item);
        partial void OnAfterClientesDeleted(Servicing.Models.sql_rendimento_consignado.Clientes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Clientes> DeleteClientes(long idcliente)
        {
            var itemToDelete = Context.Clientes
                              .Where(i => i.IdCliente == idcliente)
                              .Include(i => i.EnderecosCliente)
                              .Include(i => i.Operacoes)
                              .Include(i => i.TelefonesCliente)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnClientesDeleted(itemToDelete);


            Context.Clientes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterClientesDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportEnderecosClienteToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/enderecoscliente/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/enderecoscliente/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEnderecosClienteToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/enderecoscliente/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/enderecoscliente/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEnderecosClienteRead(ref IQueryable<Servicing.Models.sql_rendimento_consignado.EnderecosCliente> items);

        public async Task<IQueryable<Servicing.Models.sql_rendimento_consignado.EnderecosCliente>> GetEnderecosCliente(Query query = null)
        {
            var items = Context.EnderecosCliente.AsQueryable();

            items = items.Include(i => i.Clientes);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnEnderecosClienteRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEnderecosClienteGet(Servicing.Models.sql_rendimento_consignado.EnderecosCliente item);
        partial void OnGetEnderecosClienteByIdEndereco(ref IQueryable<Servicing.Models.sql_rendimento_consignado.EnderecosCliente> items);


        public async Task<Servicing.Models.sql_rendimento_consignado.EnderecosCliente> GetEnderecosClienteByIdEndereco(long idendereco)
        {
            var items = Context.EnderecosCliente
                              .AsNoTracking()
                              .Where(i => i.IdEndereco == idendereco);

            items = items.Include(i => i.Clientes);
 
            OnGetEnderecosClienteByIdEndereco(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnEnderecosClienteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEnderecosClienteCreated(Servicing.Models.sql_rendimento_consignado.EnderecosCliente item);
        partial void OnAfterEnderecosClienteCreated(Servicing.Models.sql_rendimento_consignado.EnderecosCliente item);

        public async Task<Servicing.Models.sql_rendimento_consignado.EnderecosCliente> CreateEnderecosCliente(Servicing.Models.sql_rendimento_consignado.EnderecosCliente enderecoscliente)
        {
            OnEnderecosClienteCreated(enderecoscliente);

            var existingItem = Context.EnderecosCliente
                              .Where(i => i.IdEndereco == enderecoscliente.IdEndereco)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.EnderecosCliente.Add(enderecoscliente);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(enderecoscliente).State = EntityState.Detached;
                throw;
            }

            OnAfterEnderecosClienteCreated(enderecoscliente);

            return enderecoscliente;
        }

        public async Task<Servicing.Models.sql_rendimento_consignado.EnderecosCliente> CancelEnderecosClienteChanges(Servicing.Models.sql_rendimento_consignado.EnderecosCliente item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEnderecosClienteUpdated(Servicing.Models.sql_rendimento_consignado.EnderecosCliente item);
        partial void OnAfterEnderecosClienteUpdated(Servicing.Models.sql_rendimento_consignado.EnderecosCliente item);

        public async Task<Servicing.Models.sql_rendimento_consignado.EnderecosCliente> UpdateEnderecosCliente(long idendereco, Servicing.Models.sql_rendimento_consignado.EnderecosCliente enderecoscliente)
        {
            OnEnderecosClienteUpdated(enderecoscliente);

            var itemToUpdate = Context.EnderecosCliente
                              .Where(i => i.IdEndereco == enderecoscliente.IdEndereco)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(enderecoscliente);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEnderecosClienteUpdated(enderecoscliente);

            return enderecoscliente;
        }

        partial void OnEnderecosClienteDeleted(Servicing.Models.sql_rendimento_consignado.EnderecosCliente item);
        partial void OnAfterEnderecosClienteDeleted(Servicing.Models.sql_rendimento_consignado.EnderecosCliente item);

        public async Task<Servicing.Models.sql_rendimento_consignado.EnderecosCliente> DeleteEnderecosCliente(long idendereco)
        {
            var itemToDelete = Context.EnderecosCliente
                              .Where(i => i.IdEndereco == idendereco)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEnderecosClienteDeleted(itemToDelete);


            Context.EnderecosCliente.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEnderecosClienteDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLotesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/lotes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/lotes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLotesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/lotes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/lotes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLotesRead(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Lotes> items);

        public async Task<IQueryable<Servicing.Models.sql_rendimento_consignado.Lotes>> GetLotes(Query query = null)
        {
            var items = Context.Lotes.AsQueryable();

            items = items.Include(i => i.Cedentes);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLotesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLotesGet(Servicing.Models.sql_rendimento_consignado.Lotes item);
        partial void OnGetLotesByIdLote(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Lotes> items);


        public async Task<Servicing.Models.sql_rendimento_consignado.Lotes> GetLotesByIdLote(long idlote)
        {
            var items = Context.Lotes
                              .AsNoTracking()
                              .Where(i => i.IdLote == idlote);

            items = items.Include(i => i.Cedentes);
 
            OnGetLotesByIdLote(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLotesGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLotesCreated(Servicing.Models.sql_rendimento_consignado.Lotes item);
        partial void OnAfterLotesCreated(Servicing.Models.sql_rendimento_consignado.Lotes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Lotes> CreateLotes(Servicing.Models.sql_rendimento_consignado.Lotes lotes)
        {
            OnLotesCreated(lotes);

            var existingItem = Context.Lotes
                              .Where(i => i.IdLote == lotes.IdLote)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Lotes.Add(lotes);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(lotes).State = EntityState.Detached;
                throw;
            }

            OnAfterLotesCreated(lotes);

            return lotes;
        }

        public async Task<Servicing.Models.sql_rendimento_consignado.Lotes> CancelLotesChanges(Servicing.Models.sql_rendimento_consignado.Lotes item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLotesUpdated(Servicing.Models.sql_rendimento_consignado.Lotes item);
        partial void OnAfterLotesUpdated(Servicing.Models.sql_rendimento_consignado.Lotes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Lotes> UpdateLotes(long idlote, Servicing.Models.sql_rendimento_consignado.Lotes lotes)
        {
            OnLotesUpdated(lotes);

            var itemToUpdate = Context.Lotes
                              .Where(i => i.IdLote == lotes.IdLote)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(lotes);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLotesUpdated(lotes);

            return lotes;
        }

        partial void OnLotesDeleted(Servicing.Models.sql_rendimento_consignado.Lotes item);
        partial void OnAfterLotesDeleted(Servicing.Models.sql_rendimento_consignado.Lotes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Lotes> DeleteLotes(long idlote)
        {
            var itemToDelete = Context.Lotes
                              .Where(i => i.IdLote == idlote)
                              .Include(i => i.Operacoes)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLotesDeleted(itemToDelete);


            Context.Lotes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLotesDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportOperacoesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/operacoes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/operacoes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportOperacoesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/operacoes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/operacoes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnOperacoesRead(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Operacoes> items);

        public async Task<IQueryable<Servicing.Models.sql_rendimento_consignado.Operacoes>> GetOperacoes(Query query = null)
        {
            var items = Context.Operacoes.AsQueryable();

            items = items.Include(i => i.Clientes);
            items = items.Include(i => i.Lotes);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnOperacoesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnOperacoesGet(Servicing.Models.sql_rendimento_consignado.Operacoes item);
        partial void OnGetOperacoesByIdOperacao(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Operacoes> items);


        public async Task<Servicing.Models.sql_rendimento_consignado.Operacoes> GetOperacoesByIdOperacao(long idoperacao)
        {
            var items = Context.Operacoes
                              .AsNoTracking()
                              .Where(i => i.IdOperacao == idoperacao);

            items = items.Include(i => i.Clientes);
            items = items.Include(i => i.Lotes);
 
            OnGetOperacoesByIdOperacao(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnOperacoesGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnOperacoesCreated(Servicing.Models.sql_rendimento_consignado.Operacoes item);
        partial void OnAfterOperacoesCreated(Servicing.Models.sql_rendimento_consignado.Operacoes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Operacoes> CreateOperacoes(Servicing.Models.sql_rendimento_consignado.Operacoes operacoes)
        {
            OnOperacoesCreated(operacoes);

            var existingItem = Context.Operacoes
                              .Where(i => i.IdOperacao == operacoes.IdOperacao)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Operacoes.Add(operacoes);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(operacoes).State = EntityState.Detached;
                throw;
            }

            OnAfterOperacoesCreated(operacoes);

            return operacoes;
        }

        public async Task<Servicing.Models.sql_rendimento_consignado.Operacoes> CancelOperacoesChanges(Servicing.Models.sql_rendimento_consignado.Operacoes item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnOperacoesUpdated(Servicing.Models.sql_rendimento_consignado.Operacoes item);
        partial void OnAfterOperacoesUpdated(Servicing.Models.sql_rendimento_consignado.Operacoes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Operacoes> UpdateOperacoes(long idoperacao, Servicing.Models.sql_rendimento_consignado.Operacoes operacoes)
        {
            OnOperacoesUpdated(operacoes);

            var itemToUpdate = Context.Operacoes
                              .Where(i => i.IdOperacao == operacoes.IdOperacao)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(operacoes);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterOperacoesUpdated(operacoes);

            return operacoes;
        }

        partial void OnOperacoesDeleted(Servicing.Models.sql_rendimento_consignado.Operacoes item);
        partial void OnAfterOperacoesDeleted(Servicing.Models.sql_rendimento_consignado.Operacoes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Operacoes> DeleteOperacoes(long idoperacao)
        {
            var itemToDelete = Context.Operacoes
                              .Where(i => i.IdOperacao == idoperacao)
                              .Include(i => i.Transacoes)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnOperacoesDeleted(itemToDelete);


            Context.Operacoes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterOperacoesDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTelefonesClienteToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/telefonescliente/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/telefonescliente/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTelefonesClienteToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/telefonescliente/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/telefonescliente/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTelefonesClienteRead(ref IQueryable<Servicing.Models.sql_rendimento_consignado.TelefonesCliente> items);

        public async Task<IQueryable<Servicing.Models.sql_rendimento_consignado.TelefonesCliente>> GetTelefonesCliente(Query query = null)
        {
            var items = Context.TelefonesCliente.AsQueryable();

            items = items.Include(i => i.Clientes);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTelefonesClienteRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTelefonesClienteGet(Servicing.Models.sql_rendimento_consignado.TelefonesCliente item);
        partial void OnGetTelefonesClienteByIdTelefone(ref IQueryable<Servicing.Models.sql_rendimento_consignado.TelefonesCliente> items);


        public async Task<Servicing.Models.sql_rendimento_consignado.TelefonesCliente> GetTelefonesClienteByIdTelefone(long idtelefone)
        {
            var items = Context.TelefonesCliente
                              .AsNoTracking()
                              .Where(i => i.IdTelefone == idtelefone);

            items = items.Include(i => i.Clientes);
 
            OnGetTelefonesClienteByIdTelefone(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTelefonesClienteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTelefonesClienteCreated(Servicing.Models.sql_rendimento_consignado.TelefonesCliente item);
        partial void OnAfterTelefonesClienteCreated(Servicing.Models.sql_rendimento_consignado.TelefonesCliente item);

        public async Task<Servicing.Models.sql_rendimento_consignado.TelefonesCliente> CreateTelefonesCliente(Servicing.Models.sql_rendimento_consignado.TelefonesCliente telefonescliente)
        {
            OnTelefonesClienteCreated(telefonescliente);

            var existingItem = Context.TelefonesCliente
                              .Where(i => i.IdTelefone == telefonescliente.IdTelefone)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TelefonesCliente.Add(telefonescliente);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(telefonescliente).State = EntityState.Detached;
                throw;
            }

            OnAfterTelefonesClienteCreated(telefonescliente);

            return telefonescliente;
        }

        public async Task<Servicing.Models.sql_rendimento_consignado.TelefonesCliente> CancelTelefonesClienteChanges(Servicing.Models.sql_rendimento_consignado.TelefonesCliente item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTelefonesClienteUpdated(Servicing.Models.sql_rendimento_consignado.TelefonesCliente item);
        partial void OnAfterTelefonesClienteUpdated(Servicing.Models.sql_rendimento_consignado.TelefonesCliente item);

        public async Task<Servicing.Models.sql_rendimento_consignado.TelefonesCliente> UpdateTelefonesCliente(long idtelefone, Servicing.Models.sql_rendimento_consignado.TelefonesCliente telefonescliente)
        {
            OnTelefonesClienteUpdated(telefonescliente);

            var itemToUpdate = Context.TelefonesCliente
                              .Where(i => i.IdTelefone == telefonescliente.IdTelefone)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(telefonescliente);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTelefonesClienteUpdated(telefonescliente);

            return telefonescliente;
        }

        partial void OnTelefonesClienteDeleted(Servicing.Models.sql_rendimento_consignado.TelefonesCliente item);
        partial void OnAfterTelefonesClienteDeleted(Servicing.Models.sql_rendimento_consignado.TelefonesCliente item);

        public async Task<Servicing.Models.sql_rendimento_consignado.TelefonesCliente> DeleteTelefonesCliente(long idtelefone)
        {
            var itemToDelete = Context.TelefonesCliente
                              .Where(i => i.IdTelefone == idtelefone)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTelefonesClienteDeleted(itemToDelete);


            Context.TelefonesCliente.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTelefonesClienteDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTransacoesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/transacoes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/transacoes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTransacoesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql_rendimento_consignado/transacoes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql_rendimento_consignado/transacoes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTransacoesRead(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Transacoes> items);

        public async Task<IQueryable<Servicing.Models.sql_rendimento_consignado.Transacoes>> GetTransacoes(Query query = null)
        {
            var items = Context.Transacoes.AsQueryable();

            items = items.Include(i => i.Operacoes);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTransacoesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTransacoesGet(Servicing.Models.sql_rendimento_consignado.Transacoes item);
        partial void OnGetTransacoesByIdTransacao(ref IQueryable<Servicing.Models.sql_rendimento_consignado.Transacoes> items);


        public async Task<Servicing.Models.sql_rendimento_consignado.Transacoes> GetTransacoesByIdTransacao(long idtransacao)
        {
            var items = Context.Transacoes
                              .AsNoTracking()
                              .Where(i => i.IdTransacao == idtransacao);

            items = items.Include(i => i.Operacoes);
 
            OnGetTransacoesByIdTransacao(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTransacoesGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTransacoesCreated(Servicing.Models.sql_rendimento_consignado.Transacoes item);
        partial void OnAfterTransacoesCreated(Servicing.Models.sql_rendimento_consignado.Transacoes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Transacoes> CreateTransacoes(Servicing.Models.sql_rendimento_consignado.Transacoes transacoes)
        {
            OnTransacoesCreated(transacoes);

            var existingItem = Context.Transacoes
                              .Where(i => i.IdTransacao == transacoes.IdTransacao)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Transacoes.Add(transacoes);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(transacoes).State = EntityState.Detached;
                throw;
            }

            OnAfterTransacoesCreated(transacoes);

            return transacoes;
        }

        public async Task<Servicing.Models.sql_rendimento_consignado.Transacoes> CancelTransacoesChanges(Servicing.Models.sql_rendimento_consignado.Transacoes item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTransacoesUpdated(Servicing.Models.sql_rendimento_consignado.Transacoes item);
        partial void OnAfterTransacoesUpdated(Servicing.Models.sql_rendimento_consignado.Transacoes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Transacoes> UpdateTransacoes(long idtransacao, Servicing.Models.sql_rendimento_consignado.Transacoes transacoes)
        {
            OnTransacoesUpdated(transacoes);

            var itemToUpdate = Context.Transacoes
                              .Where(i => i.IdTransacao == transacoes.IdTransacao)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(transacoes);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTransacoesUpdated(transacoes);

            return transacoes;
        }

        partial void OnTransacoesDeleted(Servicing.Models.sql_rendimento_consignado.Transacoes item);
        partial void OnAfterTransacoesDeleted(Servicing.Models.sql_rendimento_consignado.Transacoes item);

        public async Task<Servicing.Models.sql_rendimento_consignado.Transacoes> DeleteTransacoes(long idtransacao)
        {
            var itemToDelete = Context.Transacoes
                              .Where(i => i.IdTransacao == idtransacao)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTransacoesDeleted(itemToDelete);


            Context.Transacoes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTransacoesDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}