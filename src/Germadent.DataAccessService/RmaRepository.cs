using System.Collections.Generic;
using System.IO;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Newtonsoft.Json;

namespace Germadent.DataAccessService
{
    public class RmaRepository : IRmaRepository
    {
        private const string LabOrdersDataFile = "LabOrders.txt";
        private const string MillingCenterOrdersDataFile = "MCOrders.txt";

        private readonly List<LaboratoryOrder> _labOrders = new List<LaboratoryOrder>();
        private readonly List<MillingCenterOrder> _mcOrders = new List<MillingCenterOrder>();

        private List<Material> _materials = new List<Material>();

        public RmaRepository()
        {
            if (File.Exists(LabOrdersDataFile))
            {
                _labOrders = new List<LaboratoryOrder>(File.ReadAllText(LabOrdersDataFile).DeserializeFromJson<LaboratoryOrder[]>());
            }

            if (File.Exists(MillingCenterOrdersDataFile))
            {
                _mcOrders = new List<MillingCenterOrder>(File.ReadAllText(MillingCenterOrdersDataFile).DeserializeFromJson<MillingCenterOrder[]>());
            }

            FillMaterials();
        }

        public Order GetOrderDetails(int id)
        {
            return _labOrders.FirstOrDefault(x => x.Id == id);
        }

        public Order[] GetOrders(OrdersFilter filter)
        {
            if (filter.IsNullOrEmpty())
                return _labOrders.ToArray();

            return _labOrders.Where(x => x.MatchByFilter(filter)).ToArray();
        }

        public void AddLabOrder(LaboratoryOrder laboratoryOrder)
        {
            laboratoryOrder.Id = _labOrders.Count + 1;
            _labOrders.Add(laboratoryOrder);

            SaveLabOrders();
        }

        public void UpdateLabOrder(LaboratoryOrder laboratoryOrder)
        {
            var oldOrder = _labOrders.First(x => x.Id == laboratoryOrder.Id);
            var positionToUpdate = _labOrders.IndexOf(oldOrder);

            _labOrders[positionToUpdate] = laboratoryOrder;

            SaveLabOrders();
        }

        public void AddMcOrder(MillingCenterOrder millingCenterOrder)
        {
            throw new System.NotImplementedException();
        }

        public Material[] GetMaterials()
        {
            return _materials.ToArray();
        }

        private void SaveLabOrders()
        {
            File.WriteAllText(LabOrdersDataFile, _labOrders.SerializeToJson(Formatting.Indented));
        }

        private void FillMaterials()
        {
            _materials = new List<Material>
            {
                new Material{Name = "ZrO"},
                new Material{Name = "PMMA mono"},
                new Material{Name = "PMMA multi"},
                new Material{Name = "WAX"},
                new Material{Name = "MIK"},

                new Material{Name = "CAD-Temp mono"},
                new Material{Name = "CAD-Temp multi"},
                new Material{Name = "Enamic mono"},
                new Material{Name = "Enamic multi"},
                new Material{Name = "SUPRINITY"},

                new Material{Name = "MARK II"},
                new Material{Name = "TriLuxe forte"},
                new Material{Name = "Ti"},
                new Material{Name = "E.Max"},
            };
        }
    }
}