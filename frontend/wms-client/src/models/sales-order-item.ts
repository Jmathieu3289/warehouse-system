interface SalesOrderItem {
    id?: number,
    salesOrderId?: number,
    purchaseOrderItemId?: number,
    name?: string,
    quantity: number,
    unitPrice: number
}

export default SalesOrderItem;