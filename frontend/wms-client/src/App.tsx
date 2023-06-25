import { Routes, Route, Outlet, Link } from "react-router-dom";
import './App.css';
import { Navbar, Container, Nav, Button, Form, Modal, Table } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { useState } from "react";

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

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const handleSave = () => {
    setShow(false);
  };

  return (
    <Container>
      <h2>Warehouse</h2>
      <hr className="w-25 mb-4"></hr>
      <div>
        <h4>Current Pallet Bays</h4>
        <div className="border rounded p-4 d-flex flex-column align-items-center">
          <div className="text-center">
            <p className="display-4 text-secondary">No Pallet Bays</p>
            <Button variant="primary" onClick={handleShow}>Add Pallet Bays to Start</Button>
          </div>
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
          <Form className="d-flex flex-column align-items-center">
            <Form.Group className="mb-3 w-100" controlId="formRow">
              <Form.Label>Row</Form.Label>
              <Form.Control type="text" className="bg-dark-subtle" autoFocus />
              <Form.Text className="text-muted">
                Typically one or two letters. E.g. "AA"
              </Form.Text>
            </Form.Group>
            <Form.Group className="mb-3 w-100" controlId="formSections">
              <Form.Label>Sections</Form.Label>
              <Form.Control type="number" min="1" className="bg-dark-subtle" />
            </Form.Group>
            <Form.Group className="mb-3 w-100" controlId="formFloors">
              <Form.Label>Floors</Form.Label>
              <Form.Control type="number" min="1" className="bg-dark-subtle" />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" tabIndex={-1} onClick={handleClose}>Close</Button>
          <Button variant="success" onClick={handleSave}>Save Pallet Bays</Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
}

function Items() {

  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const handleSave = () => {
    setShow(false);
  };

  return (
    <Container>
      <h2>Items</h2>
      <hr className="w-25 mb-4"></hr>
      <div>
        <h4>Current Items</h4>
        <div className="border rounded p-4 d-flex flex-column align-items-center">
          <div className="text-center">
            <p className="display-4 text-secondary">No Items</p>
            <Button variant="primary" onClick={handleShow}>Add Items to Start</Button>
          </div>
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
          <Form className="d-flex flex-column align-items-center">
            <Form.Group className="mb-3 w-100" controlId="formName">
              <Form.Label>Name</Form.Label>
              <Form.Control type="text" className="bg-dark-subtle" autoFocus />
            </Form.Group>
            <Form.Group className="mb-3 w-100" controlId="formUPC">
              <Form.Label>UPC</Form.Label>
              <Form.Control type="text" className="bg-dark-subtle" />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" tabIndex={-1} onClick={handleClose}>Close</Button>
          <Button variant="success" onClick={handleSave}>Save Item</Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
}

function PurchaseOrders() {

  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const handleSave = () => {
    setShow(false);
  };

  return (
    <Container>
      <h2>Purchase Orders</h2>
      <hr className="w-25 mb-4"></hr>
      <div>
        <h4>Current Purchase Orders</h4>
        <div className="border rounded p-4 d-flex flex-column align-items-center">
          <div className="text-center">
            <p className="display-4 text-secondary">No Purchase Orders</p>
            <Button variant="primary" onClick={handleShow}>Create Purchase Orders to Start</Button>
          </div>
        </div>
      </div>
      <div className="mt-4">
        <div className="mt-3">
          <Button variant="primary" type="button" className="px-4" onClick={handleShow}>
            New Purchase Order
          </Button>
        </div>
      </div>
      <Modal show={show} onHide={handleClose} animation={false} size="xl" centered>
        <Modal.Header closeButton>
          <Modal.Title>New Purchase Order</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form className="d-flex flex-column align-items-center">
            <div className="w-100 text-left mb-2">
              Order Items
              <Table bordered size="sm">
                <thead>
                  <tr>
                    <th>Item</th>
                    <th>Qty</th>
                    <th>Unit Price</th>
                    <th>Total Purchase Price</th>
                    <th>Markup</th>
                    <th>Sell Price</th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td>Small Widget</td>
                    <td><Form.Control type="number" className="bg-dark-subtle" value="30"></Form.Control></td>
                    <td><Form.Control type="number" className="bg-dark-subtle" value="9.95"></Form.Control></td>
                    <td className="align-middle fs-5 text-center">$298.50</td>
                    <td><Form.Control type="number" className="bg-dark-subtle" value="0.00"></Form.Control></td>
                    <td className="align-middle fs-5 text-center">$9.95</td>
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

function SalesOrders() {
  return (
    <Container>
      <h2>Sales Orders</h2>
      <hr className="w-25 mb-4"></hr>
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
