import { Link } from "react-router-dom";
import "../../styles/App.css";

export default function Header() {
  return (
    <header className="main-header">
        <nav className="nav-bar">
            <div className="nav-group">
                <Link to="/tasks" className="nav-link">Tasks</Link>
            </div>

            <Link to="/" className="nav-title">Eventure</Link>

            <div className="nav-group">
                <Link to="/account" className="nav-link">Account</Link>
            </div>
        </nav>
    </header>
  );
}
