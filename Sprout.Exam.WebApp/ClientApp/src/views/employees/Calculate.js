import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';

export class EmployeeCalculate extends Component {
    static displayName = EmployeeCalculate.name;

    constructor(props) {
        super(props);
        this.state = { id: 0, fullName: '', birthdate: '', tin: '', salary: 0, typeId: 1, daysAbsent: 0, daysWorked: 0, netIncome: 0, loading: true, loadingCalculate: false };
    }

    componentDidMount() {
        this.getEmployee(this.props.match.params.id);
    }
    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleSubmit(e) {
        e.preventDefault();
        this.calculateSalary();
    }

    render() {

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <div>
                <form>
                    <div className='form-row'>
                        <div className='form-group col-md-6'>
                            <label>Full Name: <b>{this.state.fullName}</b></label>
                        </div>
                        <div className='form-group col-md-6'>
                            <label >Birthdate: <b>{this.state.birthDateShort}</b></label>
                        </div>
                    </div>

                    <div className="form-row">
                        <div className='form-group col-md-6'>
                            <label>TIN: <b>{this.state.tin}</b></label>
                        </div>
                        <div className='form-group col-md-6'>
                            <label>Employee Type: <b>{this.state.typeId === 1 ? "Regular" : "Contractual"}</b></label>
                        </div>
                    </div>

                    {this.state.typeId === 1 ?
                        <div className="form-row">
                            <div className='form-group col-md-6'><label>Salary: <b>{new Intl.NumberFormat().format(this.state.salary)} </b></label></div>
                            <div className='form-group col-md-6'><label>Tax: <b>12%</b> </label></div>
                        </div> : <div className="form-row">
                            <div className='form-group col-md-6'><label>Rate Per Day: <b>{this.state.salary}</b> </label></div>
                        </div>}

                    <div className="form-row">

                        {this.state.typeId === 1 ?
                            <div className='form-group col-md-6'>
                                <label htmlFor='inputDaysAbsent4'>Absent Days: </label>
                                <input type='text' className='form-control' id='inputDaysAbsent4' onChange={this.handleChange.bind(this)} value={this.state.daysAbsent} name="daysAbsent" placeholder='Absent Days' />
                            </div> :
                            <div className='form-group col-md-6'>
                                <label htmlFor='inputWorkDays4'>Worked Days: </label>
                                <input type='text' className='form-control' id='inputWorkDays4' onChange={this.handleChange.bind(this)} value={this.state.daysWorked} name="daysWorked" placeholder='Worked Days' />
                            </div>
                        }
                    </div>

                    <div className="form-row">
                        <div className='form-group col-md-12'>
                            <label>Net Income: <b>{this.state.netIncome}</b></label>
                        </div>
                    </div>

                    <button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingCalculate} className="btn btn-primary mr-2">{this.state.loadingCalculate ? "Loading..." : "Calculate"}</button>
                    <button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-primary">Back</button>
                </form>
            </div>;


        return (
            <div>
                <h1 id="tabelLabel" >Employee Calculate Salary</h1>
                <br />
                {contents}
            </div>
        );
    }

    async calculateSalary() {
        this.setState({ loadingCalculate: true });
        const token = await authService.getAccessToken();
        const requestOptions = {
            method: 'POST',
            headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
            body: JSON.stringify({ id: this.state.id, daysAbsent: this.state.daysAbsent, daysWorked: this.state.daysWorked })
        };
        const response = await fetch('api/employees/' + this.state.id + '/calculate', requestOptions);
        if (response.status === 200) {
            const data = await response.json();
            var nf = new Intl.NumberFormat();
            this.setState({ loadingCalculate: false, netIncome: nf.format(data) });
        }
        else {
            alert("There was an error occured.");
            this.setState({ loading: false, loadingCalculate: false });
        }


    }

    async getEmployee(id) {
        this.setState({ loading: true, loadingCalculate: false });
        const token = await authService.getAccessToken();
        const response = await fetch('api/employees/' + id, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        if (response.status === 200) {
            const data = await response.json();
            this.setState({ id: data.id, fullName: data.fullName, birthdate: data.birthdate, birthDateShort: data.birthDateShort, tin: data.tin, salary: data.salary, typeId: data.typeId, loading: false, loadingCalculate: false });
        }
        else {
            alert("There was an error occured.");
            this.setState({ loading: false, loadingCalculate: false });
        }
    }
}
