import PurchaseOrderItem from "./purchase-order-item";
import PurchaseOrderStatus from "./purchase-order-status";

interface PurchaseOrder {
    id?: number;
    status?: PurchaseOrderStatus;
    purchaseOrderItems: PurchaseOrderItem[];
    dateCreated?: string;
    dateEstimatedDelivery?: string;
    dateReceived?: string;
    dateLastModified?: string;
    comments: string;
}

export default PurchaseOrder;