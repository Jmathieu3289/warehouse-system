import { Routes, Route, Outlet, Link } from "react-router-dom";
import './App.css';
import { Navbar, Container, Nav, Button, Form, Modal, Table, Row } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import React, { Fragment, useEffect, useState } from "react";

function App() {
  return (
    <div>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="warehouse" element={<Warehouse />} />
          <Route path="items" element={<Items />} />
          <Route path="purchase-orders" element={<PurchaseOrders />} />
          <Route path="sales-orders" element={<SalesOrders />} />
          <Route path="*" element={<NoMatch />} />
        </Route>
      </Routes>
    </div>
  );
}

function Layout() {
  return (
    <div>
      <Navbar expand="lg" className="bg-body-tertiary border-bottom">
        <Container>
          <LinkContainer to="/">
            <Navbar.Brand>
              <img
                alt=""
                src="/logo128.png"
                width="30"
                height="30"
                className="d-inline-block align-top me-1"
              />{' '}
              WMS Client
            </Navbar.Brand>
          </LinkContainer>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <LinkContainer to="/">
                <Nav.Link>Home</Nav.Link>
              </LinkContainer>
              <LinkContainer to="/warehouse">
                <Nav.Link>Warehouse</Nav.Link>
              </LinkContainer>
              <LinkContainer to="/items">
                <Nav.Link>Items</Nav.Link>
              </LinkContainer>
              <LinkContainer to="/purchase-orders">
                <Nav.Link>Purchase Orders</Nav.Link>
              </LinkContainer>
              <LinkContainer to="/sales-orders">
                <Nav.Link>Sales Orders</Nav.Link>
              </LinkContainer>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
      <main className="mt-3">
        <Outlet />
      </main>
    </div>
  );
}

function Home() {
  return (
    <Container className="col-xxl-8 px-4 py-0 py-lg-5">
      <div className="row flex-lg-row-reverse align-items-center g-0 g-lg-5 py-0 py-lg-5 mt-0 mt-lg-3">
        <div className="col-10 col-sm-8 col-lg-6">
          <img src="/hero.jpg" className="d-block mx-auto img-fluid rounded" alt="Bootstrap Themes" loading="lazy" width="400" height="450" />
        </div>
        <div className="col-lg-6">
          <h1 className="display-5 fw-bold text-body-emphasis lh-1 mb-3">Welcome to my<br></br> WMS Client Project</h1>
          <p className="lead">Utilizing <code>React</code>, <code>React-Router</code>, <code>Bootstrap</code> and <code>React-Bootstrap</code> to get up and running quickly, 
          you can set up a warehouse, build items, and create purchase orders and sales orders from inventory. Leveraging an API built with <code>EF Core</code> and <code>ASP.NET</code>.</p>
          <div className="d-grid gap-2 d-md-flex justify-content-md-center">
            <LinkContainer to="/warehouse">
              <Button className="btn btn-primary btn-lg px-5 me-md-2">Get Started</Button>
            </LinkContainer>
          </div>
        </div>
      </div>
    </Container>
  );
}

function Warehouse() {

  const [show, setShow] = useState(false);
  const [palletBays, setPalletBays] = useState([]);
  const [loading, setLoading] = useState(true);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);
  
  const fetchBays = async () => {
    setLoading(true);
    let res = await fetch('/api/palletbay', {
      method: 'GET'
    });
    let data = await res.json();
    setPalletBays(data);
    setLoading(false);
  }

  useEffect(() => {
    fetchBays();
  }, []);

  const handleSave = async (e: React.SyntheticEvent) => {
    e.preventDefault();

    const target = e.target as typeof e.target & {
      row: { value: string};
      startSection: { value: number };
      endSection: { value: number };
      startFloor: { value: number };
      endFloor: { value: number };
    }

    if (target && target.row.value.length > 0 && 
        target.startSection.value > 0 && target.endSection.value >= target.startSection.value && 
        target.startFloor.value > 0 && target.endFloor.value >= target.startFloor.value) {

      let data: any = {
        row: target.row.value,
        startSection: target.startSection.value,
        endSection: target.endSection.value,
        startFloor: target.startFloor.value,
        endFloor: target.endFloor.value
      };

      try {
        
        let res = await fetch('/api/palletbay/bulk', {
          method: 'POST',
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data)
        });
  
        if (res.status === 200 || res.status === 204) {
          fetchBays();
        } else {
          //failure
        }
      } catch (err) {
        console.log(err);
      }
    }

    setShow(false);
  };

  return (
    <Container>
      <h2>Warehouse</h2>
      <hr className="w-25 mb-4"></hr>
      <div>
        <h4>Current Pallet Bays</h4>
        <div className="p-4 d-flex flex-column align-items-center">
              { (palletBays.length === 0) && !loading &&
                  <div className="text-center">
                    <p className="display-4 text-secondary">No Pallet Bays</p>
                    <Button variant="primary" onClick={handleShow}>Add Pallet Bays to Start</Button>
                  </div>
              }
              { (palletBays.length === 0) && loading &&
                  <div className="spinner"></div>
              }
              {
                (palletBays.length > 0) &&
                  <Table size="sm" className="w-100" striped bordered>
                    <thead>
                      <tr>
                        <th className="col-1">Row</th>
                        <th className="col-1">Section</th>
                        <th className="col-1">Floor</th>
                        <th>Contents</th>
                        <th className="col-1"></th>
                      </tr>
                    </thead>
                    <tbody>
                      {
                        palletBays.map((bay: any) => {
                          return (
                            <tr key={bay.id}>
                              <td>{bay.row}</td>
                              <td>{bay.section}</td>
                              <td>{bay.floor}</td>
                              <td>{bay.pallets || ''}</td>
                              <td className="text-center"><Button variant="danger" size="sm" className="w-100">Remove</Button></td>
                            </tr>
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
            Add Pallet Bays
          </Button>
        </div>
      </div>
      <Modal show={show} onHide={handleClose} animation={false} size="sm" centered>
        <Modal.Header closeButton>
          <Modal.Title>Add Pallet Bays</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form className="d-flex flex-column align-items-center" method="post" onSubmit={handleSave} autoComplete="off">
            <Form.Group className="mb-3 w-100" controlId="formRow">
              <Form.Label>Row</Form.Label>
              <Form.Control type="text" className="bg-dark-subtle w-100" name="row" autoFocus required />
              <Form.Text className="text-muted">
                Typically one or two letters. E.g. "AA"
              </Form.Text>
            </Form.Group>
            <Row>
              <Form.Group className="mb-3 col-6" controlId="formEndSection">
                <Form.Label>Start Section</Form.Label>
                <Form.Control type="number" min="1" className="bg-dark-subtle" name="startSection" required />
              </Form.Group>
              <Form.Group className="mb-3 col-6" controlId="formEndSection">
                <Form.Label>End Section</Form.Label>
                <Form.Control type="number" min="1" className="bg-dark-subtle" name="endSection" required />
              </Form.Group>
            </Row>
            <Row>
              <Form.Group className="mb-3 col-6" controlId="formStartFloor">
                <Form.Label>Start Floor</Form.Label>
                <Form.Control type="number" min="1" className="bg-dark-subtle" name="startFloor" required />
              </Form.Group>
              <Form.Group className="mb-3 col-6" controlId="formEndFloor">
                <Form.Label>End Floor</Form.Label>
                <Form.Control type="number" min="1" className="bg-dark-subtle" name="endFloor" required />
              </Form.Group>
            </Row> 
            <div className="d-flex flex-row justify-content-end w-100">
              <Button variant="secondary" type="button" className="me-2" tabIndex={-1} onClick={handleClose}>Close</Button>
              <Button variant="success" type="submit">Save Pallet Bays</Button>
            </div>
          </Form>
        </Modal.Body>
      </Modal>
    </Container>
  );
}

function Items() {

  interface Item {
    name: string;
    upc: string;
  }

  const [show, setShow] = useState(false);
  const [items, setItems] = useState<Item[]>([]);
  const [loading, setLoading] = useState(true);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const fetchItems = async () => {
    setLoading(true);
    let res = await fetch('/api/item', {
      method: 'GET'
    });
    let data = await res.json();
    setItems(data);
    setLoading(false);
  }

  const sortItems = (field: string) => {
    setItems([...items].sort((a: any, b: any) => { return a[field] < b[field] ? -1 : a[field] === b[field] ? 0 : 1}));
  }

  useEffect(() => {
    fetchItems();
  }, []);

  const handleSave = async (e: React.SyntheticEvent) => {
    e.preventDefault();

    const target = e.target as typeof e.target & {
      name: { value: string};
      upc: { value: string };
    }

    if (target && target.name.value && target.upc.value) {

      let data: any = {
        name: target.name.value,
        upc: target.upc.value,
      };

      try {    
        let res = await fetch('/api/item', {
          method: 'POST',
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data)
        });
  
        if (res.status === 201) {
          fetchItems();
        } else {
          //failure
        }
      } catch (err) {
        console.log(err);
      }
    }

    setShow(false);
  };

  const deleteItem = async (id: string) => {
    if (id) {
      try {
        let res = await fetch(`/api/item/${id}`, {
          method: 'DELETE',
          headers: {
            "Content-Type": "application/json",
          }
        });
  
        if (res.status === 204) {
          fetchItems();
        } else {
          //failure
        }
      } catch (err) {
        console.log(err);
      }
    }
  }

  return (
    <Container>
      <h2>Items</h2>
      <hr className="w-25 mb-4"></hr>
      <div>
        <h4>Current Items</h4>
        <div className="p-4 d-flex flex-column align-items-center">
          { (items.length === 0) && !loading &&
              <div className="text-center">
                <p className="display-4 text-secondary">No Items</p>
                <Button variant="primary" onClick={handleShow}>Add Items to Start</Button>
              </div>
          }
          { (items.length === 0) && loading &&
              <div className="spinner"></div>
          }
          {
            (items.length > 0) &&
              <Table size="sm" className="w-100" striped bordered>
                <thead>
                  <tr>
                    <th className="col-3"><Button variant="link" className="px-1 py-0" onClick={() => {sortItems('name')}}>Name</Button></th>
                    <th className="col-1"><Button variant="link" className="px-1 py-0" onClick={() => {sortItems('upc')}}>UPC</Button></th>
                    <th className="col-1">Created</th>
                    <th className="col-1">Last Modified</th>
                    <th className="col-1"></th>
                  </tr>
                </thead>
                <tbody>
                  {
                    items.map((item: any) => {
                      return (
                        <tr key={item.id}>
                          <td>{item.name}</td>
                          <td>{item.upc}</td>
                          <td>{new Date(item.dateCreated).toLocaleDateString() + ' ' + new Date(item.dateCreated).toLocaleTimeString()}</td>
                          <td>{new Date(item.dateLastModified).toLocaleDateString() + ' ' + new Date(item.dateLastModified).toLocaleTimeString()}</td>
                          <td className="text-center">
                            <Button variant="secondary" size="sm" className="me-2">Edit</Button>
                            <Button variant="danger" size="sm" onClick={async () => {await deleteItem(item.id)}}>Remove</Button>
                          </td>
                        </tr>
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
            Add Item
          </Button>
        </div>
      </div>
      <Modal show={show} onHide={handleClose} animation={false} size="sm" centered>
        <Modal.Header closeButton>
          <Modal.Title>Add Item</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form className="d-flex flex-column align-items-center" onSubmit={handleSave} autoComplete="off">
            <Form.Group className="mb-3 w-100" controlId="formName">
              <Form.Label>Name</Form.Label>
              <Form.Control type="text" className="bg-dark-subtle" name="name" autoFocus />
            </Form.Group>
            <Form.Group className="mb-3 w-100" controlId="formUPC">
              <Form.Label>UPC</Form.Label>
              <Form.Control type="text" className="bg-dark-subtle" name="upc" />
            </Form.Group>
            <div className="d-flex flex-row justify-content-end w-100">
              <Button variant="secondary" type="button" className="me-2" tabIndex={-1} onClick={handleClose}>Close</Button>
              <Button variant="success" type="submit">Save Item</Button>
            </div>
          </Form>
        </Modal.Body>
      </Modal>
    </Container>
  );
}

function PurchaseOrders() {

  enum PurchaseOrderStatus {
    Submitted = 0,
    Received = 1,
    Closed = 2,
    Cancelled = -1
  }

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

  interface PurchaseOrderForm {
    dateEstimatedDelivery: { value: string },
    comments: { value: string }
  }

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

  interface PurchaseOrderPutawayItem {
    id: number,
    purchasedQuantity: number,
    unitPrice: number,
    markupPrice: number,
    name: string,
    row: string,
    section: number,
    floor: number
  }

  interface Item {
    id: number,
    name: string,
    upc: string,
    dateCreated?: string,
    dateLastModified?: string
  }

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
    setLoading(true);
    let res = await fetch('/api/item', {
      method: 'GET'
    });
    let data = await res.json();
    setItems(data);
    setLoading(false);
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

    console.log(po.comments.value);

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
  
        if (res.status === 200) {
          fetchPurchaseOrders();
        } else {
          //failure
        }
      } catch (err) {
        console.log(err);
      }
    }

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

      console.log(poi);

      // Get bay
      let bay = palletBays.find((b: any) => {
        return b.row == poi.row && 
               b.section == poi.section &&
               b.floor == poi.floor;
      });

      console.log(bay);

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

        console.log(savedPallet);

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
                                            <td>{poi.sellPrice}</td>
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

function SalesOrders() {

  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const handleSave = () => {
    setShow(false);
  };

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
                    <th className="col-6">Item</th>
                    <th className="col-1">Qty</th>
                    <th className="text-end">Unit Price</th>
                    <th className="text-end">Total</th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td className="align-middle">Small Widget</td>
                    <td><Form.Control type="number" className="bg-dark-subtle" value="30"></Form.Control></td>
                    <td className="align-middle fs-5 text-end">$9.95</td>
                    <td className="align-middle fs-5 text-end">$298.50</td>
                    <td><Button variant="danger" size="sm" className="w-100 mt-1">Remove</Button></td>
                  </tr>
                </tbody>
              </Table>
              Search
              <div className="d-flex flex-row align-items-center">
                <Form.Control type="text" autoFocus className="bg-dark-subtle me-2 flex-grow-1"></Form.Control>
                <Button variant="primary text-nowrap px-4">Add Item</Button>
              </div>
            </div>
            <hr className="w-100"></hr>
            <Form.Group className="mb-3 w-100" controlId="formDeliveryDate">
              <Form.Label>Estimated Delivery Date</Form.Label>
              <Form.Control type="date" className="w-auto bg-dark-subtle" />
            </Form.Group>
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

function NoMatch() {
  return (
    <Container>
      <h2>Nothing to see here!</h2>
      <p>
        <Link to="/">Go to the home page</Link>
      </p>
    </Container>
  );
}

export default App;
