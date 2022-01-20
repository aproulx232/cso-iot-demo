using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace cso_iot_demo
{
    public interface IRepairItemRepository
    {
        Task CompleteRepairItem(int repairOrderId, int repairItemId);
    }

    public class RepairItemRepository : IRepairItemRepository
    {
        private readonly AuctionDbContext _auctionDbContext;

        public RepairItemRepository(AuctionDbContext auctionDbContext)
        {
            _auctionDbContext = auctionDbContext;
        }

        public async Task CompleteRepairItem(int repairOrderId, int repairItemId)
        {
            var repairItem = await _auctionDbContext.RepairItems.AsNoTracking().Where(ri =>
                ri.RepairOrderId == repairOrderId && ri.RepairItemId == repairItemId).FirstOrDefaultAsync();

			await using var completeRepairItem = _auctionDbContext.Database.GetDbConnection().CreateCommand();

            completeRepairItem.CommandText = @"exec dbo.spvt_m_save_repair_item 
                @OrderID = @RepairOrderId, 
				@RepairID = @RepairItemId OUTPUT, 
				@Prev = NULL, 
				@ShopID = @ShopTypeId, 
				@Comment = '', 
				@PartsMech = 0.00, 
				@PartsMechEvent = NULL, 
				@PartsMechTranType = NULL, 
				@PartsMechPSDept = NULL, 
				@Refinish = 0.00, 
				@RefinishEvent = NULL, 
				@RefinishTranType = NULL, 
				@RefinishPSDept = NULL, 
				@Body = 0.00, 
				@BodyEvent = NULL, 
				@BodyTranType = NULL, 
				@BodyPSDept = NULL, 
				@PartsEvent = NULL, 
				@PartsTranType = NULL, 
				@PartsPSDept = NULL, 
				@SubletEvent = NULL, 
				@SubletTranType = NULL, 
				@SubletPSDept = NULL,
				@ItemAmount = 0.00, 
				@InShop = 'N', 
				@Sublet = 'N', 
				@Transport = 'N', 
				@Supplemental = 'N',
				@Rework = 'N', 
				@Chargeable = 'N', 
				@NotBillable = 'N', 
				@BillSeller = 'N', 
				@InspItem = 0, 
				@InspPart = NULL, 
				@InspDamage = NULL, 
				@InspAction = @InspectionId, 
				@InspSeverity = NULL, 
				@ContractAmount = 0.00, 
				@ContractEvent = NULL,
				@ContractTranType = NULL, 
				@ContractPSDept = NULL, 
				@NotBillReason = '', 
				@source_system_id = 1,	
				@psi_result = '',
                @Completed = 'Y',
                @CompletedDtm = @CompletedDtm";
            completeRepairItem.Parameters.Add(new SqlParameter("@RepairOrderId", SqlDbType.Int) { Value = repairOrderId, Direction = ParameterDirection.Input });
            completeRepairItem.Parameters.Add(new SqlParameter("@RepairItemId", SqlDbType.Int) { Value = repairItemId, Direction = ParameterDirection.Input });
            completeRepairItem.Parameters.Add(new SqlParameter("@ShopTypeId", SqlDbType.Int) { Value = repairItem.ShopTypeId, Direction = ParameterDirection.Input });
            completeRepairItem.Parameters.Add(new SqlParameter("@InspectionId", SqlDbType.Int) { Value = repairItem.InspActionId, Direction = ParameterDirection.Input });
            completeRepairItem.Parameters.Add(new SqlParameter("@CompletedDtm", SqlDbType.DateTime) { Value = DateTime.Now, Direction = ParameterDirection.Input });
            await _auctionDbContext.Database.OpenConnectionAsync();
            await completeRepairItem.ExecuteScalarAsync();
        }
    }
}
