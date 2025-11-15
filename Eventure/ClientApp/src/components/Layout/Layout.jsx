import Header from "./Header";

export default function Layout({ children }) {
  return (
    <>
      <Header />
      <div className="page-content">
        {children}
      </div>
    </>
  );
}
