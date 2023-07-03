import Item from "./item";
import PurchaseOrder from "./purchase-order";

interface PurchaseOrderItem {
    id?: number,
    itemId: number,
    item?: Item,
    purchaseOrderId?: number,
    purchaseOrder?: PurchaseOrder,
    palletId?: number,
    pallet?: any,
    purchasedQuantity: number,
    currentQuantity?: number,
    weight?: number,
    unitPrice: number,
    markupPrice: number,
    marginPrice?: number,
    freightPrice?: number,
    sellPrice?: number
}

export default PurchaseOrderItem;