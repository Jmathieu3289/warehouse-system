import { useState, useEffect, Fragment } from "react";
import { Container, Button, Table, Modal, Form } from "react-bootstrap";
import Swal from "sweetalert2";

import Item from "../models/item";
import PurchaseOrder from "../models/purchase-order";
import PurchaseOrderForm from "../models/purchase-order-form";
import PurchaseOrderItem from "../models/purchase-order-item";
import PurchaseOrderPutawayItem from "../models/purchase-order-putaway-item";
import PurchaseOrderStatus from "../models/purchase-order-status";

function PurchaseOrders() {

    const [show, setShow] = useState(false);
    const [loading, setLoading] = useState(true);
  
    const [purchaseOrders, setPurchaseOrders] = useState<PurchaseOrder[]>([]);
    const [purchaseOrderItems, setPurchaseOrderItems] = useState<PurchaseOrderItem[]>([]);
  
    const [items, setItems] = useState<Item[]>([]);
    const [query, setQuery] = useState('');
  
    const [receiveShow, setReceiveShow] = useState(false);
    const [purchaseOrderPutawayItems, setPurchaseOrderPutawayItems] = useState<PurchaseOrderPutawayItem[]>([]);
    const [currentPurchaseOrder, setCurrentPurchaseOrder] = useState<PurchaseOrder>();
  
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
  
    const handleReceiveClose = () => setReceiveShow(false);
    const handleReceiveShow = (purchaseOrder: PurchaseOrder) => {
      setCurrentPurchaseOrder(purchaseOrder);
      setPurchaseOrderPutawayItems(purchaseOrder.purchaseOrderItems.map(poi => {
        return {
          id: (poi.id || 0),
          purchasedQuantity: poi.purchasedQuantity,
          unitPrice: poi.unitPrice,
          markupPrice: poi.markupPrice,
          name: (poi.item?.name || ''),
          row: '',
          section: 1,
          floor: 1
        }
      }));
      setReceiveShow(true);
    }
  
  
    const fetchPurchaseOrders = async () => {
      setLoading(true);
      let res = await fetch('/api/purchaseorder', {
        method: 'GET'
      });
      let data = await res.json();
      setPurchaseOrders(data);
      setLoading(false);
    }
  
    const fetchItems = async () => {
      let res = await fetch('/api/item', {
        method: 'GET'
      });
      let data = await res.json();
      setItems(data);
    }
  
    useEffect(() => {
      fetchPurchaseOrders();
      fetchItems();
    }, []);
  
    const addPurchaseOrderItem = (item: Item) => {
      let po: PurchaseOrderItem = {
        itemId: item.id,
        item: item,
        purchasedQuantity: 0,
        unitPrice: 0,
        markupPrice: 0
      }
  
      setPurchaseOrderItems([...purchaseOrderItems, po]);
      setQuery('');
    }
  
    const updatePOIProperty = (itemToUpdate: PurchaseOrderItem, property: string, newValue: any) => {
      setPurchaseOrderItems(prevItems => prevItems.map(item => 
        item === itemToUpdate  ? {...item, [property]: newValue} : item
      ))
    }
  
    const updatePOIPutawayProperty = (itemToUpdate: PurchaseOrderPutawayItem, property: string, newValue: any) => {
      setPurchaseOrderPutawayItems(prevItems => prevItems.map(item => 
        item === itemToUpdate  ? {...item, [property]: newValue} : item
      ))
    }
  
    const handleSave = async (e: React.SyntheticEvent) => {
      e.preventDefault();
  
      const po = e.target as typeof e.target & PurchaseOrderForm;
  
      if (po && purchaseOrderItems && purchaseOrderItems.length > 0) {
  
        let data: PurchaseOrder = {
          purchaseOrderItems: purchaseOrderItems.map(poi => {
            return {
              itemId: poi.itemId,
              purchasedQuantity: poi.purchasedQuantity,
              weight: poi.weight,
              unitPrice: poi.unitPrice,
              markupPrice: poi.markupPrice,
              marginPrice: 0,
              freightPrice: 0
            }
          }),
          dateEstimatedDelivery: po.dateEstimatedDelivery.value,
          comments: po.comments.value
        };
  
        try {    
          let res = await fetch('/api/purchaseorder', {
            method: 'POST',
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(data)
          });
    
          if (res.status >= 200 && res.status < 300) {
            fetchPurchaseOrders();
          } else {
            //failure
          }
        } catch (err) {
          console.log(err);
        }
      }
  
      Swal.fire({
        title: 'Order Created',
        icon: 'success',
        toast: true,
        position: 'center',
        showConfirmButton: false,
        timer: 2000
      });
      setShow(false);
    };
  
    const deletePurchaseOrder = async (id: number | undefined) => {
      if (id) {
        try {
          let res = await fetch(`/api/purchaseorder/${id}`, {
            method: 'DELETE',
            headers: {
              "Content-Type": "application/json",
            }
          });
    
          if (res.status >= 200 && res.status < 300) {
            fetchPurchaseOrders();
          } else {
            //failure
          }
        } catch (err) {
          console.log(err);
        }
      }
    }
  
    const finishReceiveOrder = async(e: React.SyntheticEvent) => {
  
      e.preventDefault();
  
      // TODO: Implement logic in backend and add interfaces
  
      let palletBays = await (await fetch('/api/palletbay', {
        method: 'GET'
      })).json();
  
      console.log(palletBays);
   
      purchaseOrderPutawayItems.forEach(async poi => {
  
        // Get bay
        let bay = palletBays.find((b: any) => {
          return b.row == poi.row && 
                 b.section == poi.section &&
                 b.floor == poi.floor;
        });
  
        if (bay) {
          let pallet = {
            palletBayId: bay.id
          }
  
          let savedPallet = await (await fetch(`/api/pallet`, {
            method: 'POST',
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(pallet)
          })).json();
  
          await fetch(`/api/purchaseorderitem/${poi.id}`, {
            method: 'PUT',
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({
              id: poi.id,
              palletId: savedPallet.id,
              purchasedQuantity: poi.purchasedQuantity,
              unitPrice: poi.unitPrice,
              markupPrice: poi.markupPrice
            })
          })
        } else {
          return;
        }
      });
  
      if (currentPurchaseOrder)
      {
        await fetch(`/api/purchaseorder/${currentPurchaseOrder.id}`, {
          method: 'PUT',
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            id: currentPurchaseOrder.id,
            status: PurchaseOrderStatus.Received,
            dateReceived: new Date().toISOString()
          })
        });
  
        setPurchaseOrderItems([]);
        setPurchaseOrderPutawayItems([]);
        handleReceiveClose();
        Swal.fire({
          title: 'Order Received',
          icon: 'success',
          toast: true,
          position: 'center',
          showConfirmButton: false,
          timer: 2000
        });
        fetchPurchaseOrders();
      }
    
    }
  
    const getStatusName = (status: PurchaseOrderStatus | undefined) => {
      return PurchaseOrderStatus[status || 0];
    }
  
    return (
      <Container>
        <h2>Purchase Orders</h2>
        <hr className="w-25 mb-4"></hr>
        <div>
          <h4>Current Purchase Orders</h4>
          <div className="p-4 d-flex flex-column align-items-center">
            { (purchaseOrders.length === 0) && !loading &&
                <div className="text-center">
                  <p className="display-4 text-secondary">No Purchase Orders</p>
                  <Button variant="primary" onClick={handleShow}>Create A Purchase Order to Start</Button>
                </div>
            }
            { (purchaseOrders.length === 0) && loading &&
                <div className="spinner"></div>
            }
            {
              (purchaseOrders.length > 0) &&
                <Table size="sm" className="w-100" striped bordered>
                  <thead>
                    <tr>
                      <th className="col-1">Status</th>
                      <th className="col-1">Created</th>
                      <th className="col-1">Received</th>
                      <th className="col-1">Last Modified</th>
                      <th className="col-1"></th>
                    </tr>
                  </thead>
                  <tbody>
                    {
                      purchaseOrders.map((po: PurchaseOrder) => {
                        return (
                          <Fragment>
                            <tr key={po.id}>
                              <td>{getStatusName(po.status)}</td>
                              <td>{new Date(po.dateCreated || Date.now()).toLocaleDateString()}</td>
                              <td>{po.dateReceived ? new Date(po.dateReceived).toLocaleDateString() : ''}</td>
                              <td>{new Date(po.dateLastModified || Date.now()).toLocaleDateString()}</td>
                              <td className="text-center">
                                {po.status === 0 && 
                                  <Button variant="secondary" size="sm" className="me-2" onClick={() => { handleReceiveShow(po) }}>Receive</Button>
                                }
                                <Button variant="danger" size="sm" onClick={async () => {await deletePurchaseOrder(po.id)}}>Remove</Button>
                              </td>
                            </tr>
                            <tr key={po.id + '-items'}>
                              <td colSpan={4}>
                                <div className="p-2">
                                  Purchase Items
                                  <Table size="sm" className="w-100" striped bordered>
                                    <thead>
                                      <tr>
                                        <th>Item</th>
                                        <th>Order Quantity</th>
                                        <th>Current Quantity</th>
                                        <th>Unit Price</th>
                                        <th>Markup</th>
                                        <th>Sell Price</th>
                                      </tr>
                                    </thead>
                                    <tbody>
                                      {
                                        po.purchaseOrderItems.map((poi: PurchaseOrderItem) => {
                                          return (
                                            <tr key={poi.id}>
                                              <td>{poi.item?.name}</td>
                                              <td>{poi.purchasedQuantity}</td>
                                              <td>{poi.currentQuantity}</td>
                                              <td>{poi.unitPrice}</td>
                                              <td>{poi.markupPrice}</td>
                                              <td>{poi.sellPrice?.toFixed(2)}</td>
                                            </tr>
                                          )
                                        })
                                      }
                                    </tbody>
                                  </Table>
                                </div>
                              </td>
                            </tr> 
                          </Fragment>            
                        )
                      })
                    }
                  </tbody>
                </Table>
                }
          </div>
        </div>
        <div className="mt-4">
          <div className="mt-3">
            <Button variant="primary" type="button" className="px-4" onClick={handleShow}>
              Create Purchase Order
            </Button>
          </div>
        </div>
        <Modal show={show} onHide={handleClose} animation={false} backdrop="static" size="xl" centered>
          <Modal.Header closeButton>
            <Modal.Title>New Purchase Order</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form className="d-flex flex-column align-items-center" onSubmit={handleSave} autoComplete="off">
              <div className="w-100 text-left mb-2">
                Order Items
                <Table bordered size="sm">
                  <thead>
                    <tr>
                      <th className="col-5">Item</th>
                      <th className="col-1">Qty</th>
                      <th className="col-1">Unit Price</th>
                      <th className="text-end">Total</th>
                      <th className="col-1">Markup</th>
                      <th className="text-end">Sell Price</th>
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    {
                      purchaseOrderItems.map(poi => {
                        return (
                          <tr key={purchaseOrderItems.indexOf(poi)}>
                            <td className="align-middle">{poi.item?.name}</td>
                            <td><Form.Control name="qty" type="number" step="1" className="bg-dark-subtle" value={poi.purchasedQuantity} onChange={e => updatePOIProperty(poi, 'purchasedQuantity', parseInt(e.target.value) || 0)}></Form.Control></td>
                            <td><Form.Control name="unitPrice" type="number" step="0.01" className="bg-dark-subtle" value={poi.unitPrice} onChange={e => updatePOIProperty(poi, 'unitPrice', parseFloat(e.target.value) || 0)}></Form.Control></td>
                            <td className="align-middle fs-5 text-end">${(poi.unitPrice * (poi.purchasedQuantity || 0)).toFixed(2)}</td>
                            <td><Form.Control name="markupPrice" type="number" step="0.01" className="bg-dark-subtle" value={poi.markupPrice} onChange={e => updatePOIProperty(poi, 'markupPrice', parseFloat(e.target.value) || 0)}></Form.Control></td>
                            <td className="align-middle fs-5 text-end">${(poi.unitPrice + poi.markupPrice).toFixed(2)}</td>
                            <td><Button variant="danger" size="sm" className="w-100 mt-1" onClick={() => setPurchaseOrderItems(purchaseOrderItems.filter(item => item !== poi))}>Remove</Button></td>
                          </tr>
                        )
                      })
                    }
                    
                  </tbody>
                </Table>
                Find Items
                <Form.Control type="text" autoFocus className="bg-dark-subtle me-2 flex-grow-1" value={query} onChange={e => setQuery(e.target.value)}></Form.Control>
                <div className="mt-2 border rounded p-2">
                  { query.length > 2 &&
                    items
                    .filter(item => {
                      return item.name.toUpperCase().includes(query.toUpperCase())
                    })
                    .map(item => { 
                      return <div key={item.id}><Button variant="link" size="sm" onClick={() => addPurchaseOrderItem(item)}>{item.name}</Button></div>
                    })
                  }
                </div>
              </div>
              <hr className="w-100"></hr>
              <Form.Group className="mb-3 w-100" controlId="formDeliveryDate">
                <Form.Label>Estimated Delivery Date</Form.Label>
                <Form.Control name="dateEstimatedDelivery" type="date" className="w-auto bg-dark-subtle" />
              </Form.Group>
              <Form.Group className="mb-3 w-100" controlId="formComments">
                <Form.Label>Comments</Form.Label>
                <Form.Control name="comments" as="textarea" rows={3} className="bg-dark-subtle"/>
              </Form.Group>
              <div className="d-flex flex-row justify-content-end w-100">
                <Button variant="secondary" type="button" className="me-2" tabIndex={-1} onClick={handleClose}>Close</Button>
                <Button variant="success" type="submit">Save Purchase Order</Button>
              </div>
            </Form>
          </Modal.Body>
        </Modal>
        <Modal show={receiveShow} onHide={handleReceiveClose} animation={false} backdrop="static" size="lg" centered>
          <Modal.Header closeButton>
            <Modal.Title>Receive Purchase Order</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form className="d-flex flex-column align-items-center" onSubmit={finishReceiveOrder} autoComplete="off">
              <div className="w-100 text-left mb-2">
                Order Items
                <Table bordered size="sm">
                  <thead>
                    <tr>
                      <th className="col-5">Item</th>
                      <th className="col-1">Row</th>
                      <th className="col-1">Section</th>
                      <th className="col-1">Floor</th>
                    </tr>
                  </thead>
                  <tbody>
                    {
                      purchaseOrderPutawayItems.map(poi => {
                        return (
                          <tr key={purchaseOrderPutawayItems.indexOf(poi)}>
                            <td className="align-middle">{poi.name}</td>
                            <td><Form.Control name="row" className="bg-dark-subtle" value={poi.row} onChange={e => updatePOIPutawayProperty(poi, 'row', e.target.value)}></Form.Control></td>
                            <td><Form.Control name="section" type="number" step="1" className="bg-dark-subtle" value={poi.section} onChange={e => updatePOIPutawayProperty(poi, 'section', parseFloat(e.target.value) || 0)}></Form.Control></td>
                            <td><Form.Control name="floor" type="number" step="1" className="bg-dark-subtle" value={poi.floor} onChange={e => updatePOIPutawayProperty(poi, 'floor', parseFloat(e.target.value) || 0)}></Form.Control></td>
                          </tr>
                        )
                      })
                    }
                    
                  </tbody>
                </Table>
              </div>
              <hr className="w-100"></hr>
              <div className="d-flex flex-row justify-content-end w-100">
                <Button variant="secondary" type="button" className="me-2" tabIndex={-1} onClick={handleReceiveClose}>Close</Button>
                <Button variant="success" type="submit">Finish Receiving</Button>
              </div>
            </Form>
          </Modal.Body>
        </Modal>
      </Container>
    );
}

export default PurchaseOrders