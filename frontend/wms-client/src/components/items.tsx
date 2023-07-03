import { useState, useEffect } from "react";
import { Container, Button, Table, Modal, Form } from "react-bootstrap";
import Swal from "sweetalert2";
import Item from "../models/item";

function Items() {

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
      sortItems('upc');
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
  
      Swal.fire({
        title: 'Item Created',
        icon: 'success',
        toast: true,
        position: 'center',
        showConfirmButton: false,
        timer: 2000
      });
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

export default Items;