import { useState, useEffect } from "react";
import { Container, Button, Modal, Table, Form } from "react-bootstrap";

import PurchaseItem from "../models/purchase-item";
import SalesOrder from "../models/sales-order";
import SalesOrderItem from "../models/sales-order-item";

function SalesOrders() {

    const [show, setShow] = useState(false);
  
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
  
    const [salesOrders, setSalesOrders] = useState<SalesOrder[]>([]);
  
    const [salesOrderItems, setSalesOrderItems] = useState<SalesOrderItem[]>([]);
    const [loading, setLoading] = useState(false);
  
    const [query, setQuery] = useState('');
    const [items, setItems] = useState<PurchaseItem[]>([]);
  
    const fetchSalesOrders = async () => {
      setLoading(true);
      let res = await fetch('/api/salesorder', {
        method: 'GET'
      });
      let data = await res.json();
      setSalesOrders(data);
      setLoading(false);
    }
  
    const fetchPurchaseItems = async () => {
      let res = await fetch('/api/purchaseorderitem/saleable', {
        method: 'GET'
      });
      let data = await res.json();
      setItems(data);
    }
  
    useEffect(() => {
      fetchSalesOrders();
      fetchPurchaseItems();
    }, []);
  
    const handleSave = () => {
      setShow(false);
    };
  
    const addSalesOrderItem = (item: PurchaseItem) => {
      setSalesOrderItems([...salesOrderItems, {
        purchaseOrderItemId: item.purchaseOrderItemId,
        name: item.name,
        quantity: 1,
        unitPrice: item.unitPrice
      }]);
      setQuery('');
    }
  
    const updateSOIProperty = (itemToUpdate: SalesOrderItem, property: string, newValue: any) => {
      setSalesOrderItems(prevItems => prevItems.map(item => 
        item === itemToUpdate  ? {...item, [property]: newValue} : item
      ))
    }
  
    return (
      <Container>
        <h2>Sales Orders</h2>
        <hr className="w-25 mb-4"></hr>
        <div>
          <h4>Current Sales Orders</h4>
          <div className="border rounded p-4 d-flex flex-column align-items-center">
            <div className="text-center">
              <p className="display-4 text-secondary">No Sales Orders</p>
              <Button variant="primary" onClick={handleShow}>Create Sales Orders to Start</Button>
            </div>
          </div>
        </div>
        <div className="mt-4">
          <div className="mt-3">
            <Button variant="primary" type="button" className="px-4" onClick={handleShow}>
              New Sales Order
            </Button>
          </div>
        </div>
        <Modal show={show} onHide={handleClose} animation={false} size="xl" centered>
          <Modal.Header closeButton>
            <Modal.Title>New Sales Order</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form className="d-flex flex-column align-items-center">
              <div className="w-100 text-left mb-2">
                Order Items
                <Table bordered size="sm">
                  <thead>
                    <tr>
                      <th className="col-5">Item</th>
                      <th className="col-1">Qty</th>
                      <th className="col-2 text-end">Unit Price</th>
                      <th className="col-2 text-end">Total</th>
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    {
                      salesOrderItems.map(soi => {
                        return (
                          <tr key={salesOrderItems.indexOf(soi)}>
                            <td className="align-middle">{soi.name}</td>
                            <td><Form.Control type="number" className="bg-dark-subtle" value={soi.quantity} onChange={e => updateSOIProperty(soi, 'quantity', parseInt(e.target.value) || 0)}></Form.Control></td>
                            <td className="align-middle fs-5 text-end">${soi.unitPrice.toFixed(2)}</td>
                            <td className="align-middle fs-5 text-end">${(soi.unitPrice * soi.quantity).toFixed(2)}</td>
                            <td><Button variant="danger" size="sm" className="w-100 mt-1">Remove</Button></td>
                          </tr>
                        )
                      })
                    }    
                    <tr key="total-row">
                      <td></td>
                      <td></td>
                      <td></td>
                      <td>
                        {
                          <div className="text-end fs-5 fw-bold">
                            {
                              '$' + salesOrderItems.reduce((acc, item) => {
                                return acc + (item.quantity * item.unitPrice)
                              }, 0).toFixed(2)
                            }
                          </div> 
                        }
                      </td>
                      <td></td>
                    </tr>    
                  </tbody>
                </Table>
                Search
                <Form.Control type="text" autoFocus className="bg-dark-subtle me-2 flex-grow-1" value={query} onChange={e => setQuery(e.target.value)}></Form.Control>
                <div className="mt-2 border rounded p-2">
                  { query.length > 2 &&
                    items
                    .filter(item => {
                      return item.name.toUpperCase().includes(query.toUpperCase())
                    })
                    .map(item => { 
                      return <div key={item.purchaseOrderItemId}><Button variant="link" size="sm" onClick={() => addSalesOrderItem(item)}>{item.name + ' | $' + item.unitPrice + ' | ' + item.quantity + ' available'}</Button></div>
                    })
                  }
                </div>
              </div>
              <hr className="w-100"></hr>
              <Form.Group className="mb-3 w-100" controlId="formComments">
                <Form.Label>Comments</Form.Label>
                <Form.Control as="textarea" rows={3} className="bg-dark-subtle" />
              </Form.Group>
            </Form>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" tabIndex={-1} onClick={handleClose}>Close</Button>
            <Button variant="success" onClick={handleSave}>Save Purchase Order</Button>
          </Modal.Footer>
        </Modal>
      </Container>
    );
}

export default SalesOrders